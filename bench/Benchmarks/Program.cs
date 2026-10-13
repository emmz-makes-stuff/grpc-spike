using BenchmarkDotNet.Running;
using GrpcSpike.Benchmarks;

// One runtime per run (§4 decision): `--target net10.0|net11.0` picks the job, and the curated exports land in
// docs/analysis/results/<target>/. `--attribute` records and summarises GcVerbose traces instead (task 4.6); those
// runs carry profiler overhead, so their exports are not copied. Every other argument goes to BenchmarkDotNet.
const string AttributeOption = "--attribute";

var (target, benchmarkArgs) = BenchmarkTarget.Parse(args);
var attribute = benchmarkArgs.Contains(AttributeOption);
benchmarkArgs = [.. benchmarkArgs.Where(a => a != AttributeOption)];

var started = DateTime.UtcNow;
var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
    .Run(benchmarkArgs, SpikeConfig.Create(target, attribute))
    .ToList();

if (attribute)
{
    var traces = Directory.EnumerateFiles(SpikeConfig.ArtifactsPath(target), "*.nettrace", SearchOption.AllDirectories)
        .Where(t => File.GetLastWriteTimeUtc(t) >= started)
        .ToList();
    if (traces.Count == 0)
    {
        Console.Error.WriteLine("// * --attribute recorded no .nettrace files.");
        return 1;
    }

    foreach (var trace in traces)
    {
        var summary = AllocationAttribution.Summarise(trace);
        File.WriteAllText(Path.ChangeExtension(trace, ".summary.md"), summary);
        Console.WriteLine(summary);
        Console.WriteLine($"// * Attribution summary written to {Path.ChangeExtension(trace, ".summary.md")}");
    }
}
else
{
    ResultsExport.Write(summaries, target, Path.Combine(RepositoryRoot.Find(), "docs", "analysis", "results", target.Moniker));
}

// BenchmarkSwitcher reports bad arguments and failed benchmarks on the console but still exits 0.
var reports = summaries.SelectMany(s => s.Reports).ToList();
if (reports.Count == 0 || reports.Any(r => !r.Success) || summaries.Any(s => s.HasCriticalValidationErrors))
{
    Console.Error.WriteLine("// * No benchmark ran, or one failed: see the output above.");
    return 1;
}

return 0;
