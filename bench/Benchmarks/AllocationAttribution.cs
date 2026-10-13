using System.Globalization;
using System.Text;
using Microsoft.Diagnostics.Tracing.Etlx;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// Summarises an <c>--attribute</c> run's GcVerbose trace (task 4.6, design D3): allocated bytes by type, and by the
/// method that allocated them. GC allocation ticks are sampled (about one per 100 KB per heap), so the shares are
/// estimates; the trace covers the whole benchmark process, harness included.
/// </summary>
public static class AllocationAttribution
{
    private const int Top = 20;

    /// <summary>Frames in these modules are framework plumbing; the first frame outside them is the caller of interest.</summary>
    private static readonly string[] FrameworkModules = ["System.", "Microsoft.", "BenchmarkDotNet", "Perfolizer"];

    public static string Summarise(string tracePath)
    {
        var bytesByType = new Dictionary<string, long>();
        var bytesBySite = new Dictionary<(string Type, string Allocator, string Caller), long>();
        long total = 0;

        using (var log = new TraceLog(TraceLog.CreateFromEventPipeDataFile(tracePath)))
        {
            foreach (var tick in log.Events.ByEventType<GCAllocationTickTraceData>())
            {
                var type = string.IsNullOrEmpty(tick.TypeName) ? "(unknown type)" : tick.TypeName;
                var bytes = tick.AllocationAmount64;
                var (allocator, caller) = Frames(tick.CallStack());

                total += bytes;
                bytesByType[type] = bytesByType.GetValueOrDefault(type) + bytes;
                bytesBySite[(type, allocator, caller)] = bytesBySite.GetValueOrDefault((type, allocator, caller)) + bytes;
            }
        }

        var text = new StringBuilder();
        text.AppendLine($"# Allocation attribution: {Path.GetFileName(tracePath)}");
        text.AppendLine();
        text.AppendLine("Sampled GC allocation ticks: shares are estimates. The whole benchmark process is traced.");
        text.AppendLine();
        if (total == 0)
        {
            text.AppendLine("No allocation ticks were recorded.");
            return text.ToString();
        }

        text.AppendLine($"## Top {Top} allocated types");
        text.AppendLine();
        text.AppendLine("| Type | Sampled bytes | Share |");
        text.AppendLine("|---|---:|---:|");
        foreach (var (type, bytes) in bytesByType.OrderByDescending(p => p.Value).Take(Top))
        {
            text.AppendLine(CultureInfo.InvariantCulture, $"| `{type}` | {bytes:N0} | {Share(bytes, total)} |");
        }

        text.AppendLine();
        text.AppendLine($"## Top {Top} allocation sites");
        text.AppendLine();
        text.AppendLine("Allocator is the innermost managed frame; caller is the first frame outside the framework and harness.");
        text.AppendLine();
        text.AppendLine("| Type | Allocator | Caller | Sampled bytes | Share |");
        text.AppendLine("|---|---|---|---:|---:|");
        foreach (var (site, bytes) in bytesBySite.OrderByDescending(p => p.Value).Take(Top))
        {
            text.AppendLine(CultureInfo.InvariantCulture, $"| `{site.Type}` | `{site.Allocator}` | `{site.Caller}` | {bytes:N0} | {Share(bytes, total)} |");
        }

        return text.ToString();
    }

    private static (string Allocator, string Caller) Frames(TraceCallStack? stack)
    {
        string? allocator = null;
        for (var frame = stack; frame is not null; frame = frame.Caller)
        {
            var method = frame.CodeAddress.FullMethodName;
            if (string.IsNullOrEmpty(method))
            {
                continue;
            }

            allocator ??= method;
            var module = frame.CodeAddress.ModuleName ?? string.Empty;
            if (!FrameworkModules.Any(prefix => module.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
            {
                return (allocator, method);
            }
        }

        return (allocator ?? "(no managed frame)", "(none outside framework)");
    }

    private static string Share(long bytes, long total) => ((double)bytes / total).ToString("P1", CultureInfo.InvariantCulture);
}
