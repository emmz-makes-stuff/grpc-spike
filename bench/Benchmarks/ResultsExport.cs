using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Reports;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// Copies the curated exports into <c>docs/analysis/results/&lt;runtime&gt;/</c> and writes the
/// <c>environment.md</c> sidecar. BenchmarkDotNet's own exports record hardware, OS and runtime; the sidecar adds
/// the pinned subject versions, which they don't (design D7, D8).
/// </summary>
public static class ResultsExport
{
    private const string PinPrefix = "Pin:";
    private static readonly string[] CuratedPatterns = ["*-report-github.md", "*-report-full.json"];

    public static void Write(IEnumerable<Summary> summaries, BenchmarkTarget target, string resultsDirectory)
    {
        var ran = summaries.Where(s => s.Reports.Length > 0).ToList();
        if (ran.Count == 0)
        {
            return;
        }

        Directory.CreateDirectory(resultsDirectory);
        foreach (var file in ran.SelectMany(s => CuratedPatterns.SelectMany(p => Directory.EnumerateFiles(s.ResultsDirectoryPath, p))))
        {
            File.Copy(file, Path.Combine(resultsDirectory, Path.GetFileName(file)), overwrite: true);
        }

        File.WriteAllText(Path.Combine(resultsDirectory, "environment.md"), Describe(target, ran));
        Console.WriteLine($"// * Curated exports copied to {resultsDirectory}");
    }

    private static string Describe(BenchmarkTarget target, IEnumerable<Summary> summaries)
    {
        var text = new StringBuilder();
        text.AppendLine($"# Environment: {target.Moniker}");
        text.AppendLine();
        text.AppendLine($"Timings are specific to this machine ({Machine(summaries)}) and indicative only.");
        text.AppendLine("Allocated bytes do not depend on the machine and are the primary evidence (design D8).");
        text.AppendLine("Hardware, OS and runtime details for each run are in the `*-report-github.md` headers and the");
        text.AppendLine("`HostEnvironmentInfo` of each `*-report-full.json` (which describes the net10.0 host process, not the runtime measured).");
        text.AppendLine();
        text.AppendLine($"`{nameof(ReferenceBenchmarks)}` rows (category `{ReferenceBenchmarks.Category}`) are protobuf-net: reference-only,");
        text.AppendLine("never part of the go/no-go criteria (ADR-0002, design D8).");
        text.AppendLine();
        text.AppendLine($"- Runtime under test: `{target.Moniker}`");
        // The JSON export's HostEnvironmentInfo is the host process (always net10.0); this is the runtime measured.
        foreach (var runtime in summaries.SelectMany(s => s.Reports).Select(r => r.GetRuntimeInfo()).Distinct())
        {
            text.AppendLine($"- Runtime as measured: {runtime}");
        }

        text.AppendLine($"- Machine: {Machine(summaries)}");
        text.AppendLine();
        text.AppendLine("## Pinned packages (Directory.Packages.props)");
        text.AppendLine();
        text.AppendLine("| Package | Version |");
        text.AppendLine("|---|---|");
        foreach (var pin in Pins())
        {
            text.AppendLine($"| {pin.Package} | {pin.Version} |");
        }

        text.AppendLine();
        text.AppendLine("## Subject assemblies as loaded");
        text.AppendLine();
        text.AppendLine("| Assembly | Informational version |");
        text.AppendLine("|---|---|");
        foreach (var assembly in new[] { typeof(IMessage).Assembly, typeof(Metadata).Assembly, typeof(GrpcChannel).Assembly })
        {
            var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
            text.AppendLine($"| {assembly.GetName().Name} | {version} |");
        }

        return text.ToString();
    }

    /// <summary>The timing label: CPU, architecture and OS as BenchmarkDotNet saw them on this run.</summary>
    private static string Machine(IEnumerable<Summary> summaries)
    {
        var cpu = summaries.First().HostEnvironmentInfo.Cpu.Value.ProcessorName ?? "unknown CPU";
        return $"{cpu}, {RuntimeInformation.OSArchitecture}, {RuntimeInformation.OSDescription}";
    }

    /// <summary>Package pins stamped into this assembly by the <c>StampPackagePins</c> target in Benchmarks.csproj.</summary>
    private static IEnumerable<(string Package, string Version)> Pins() =>
        typeof(ResultsExport).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .Where(a => a.Key.StartsWith(PinPrefix, StringComparison.Ordinal))
            .Select(a => (a.Key[PinPrefix.Length..], (a.Value ?? string.Empty).Trim('[', ']')))
            .OrderBy(p => p.Item1, StringComparer.Ordinal);
}
