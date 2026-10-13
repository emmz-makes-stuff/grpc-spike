using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Json;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// Shared config (design D3, D7): allocations via <see cref="MemoryDiagnoser"/>, GitHub Markdown and JSON exports.
/// BenchmarkDotNet's working artifacts stay under the ignored BenchmarkDotNet.Artifacts/; only the curated exports
/// are copied into the repo. With <c>attribute</c>, a GcVerbose EventPipe trace is recorded per benchmark too.
/// </summary>
public static class SpikeConfig
{
    public static string ArtifactsPath(BenchmarkTarget target) =>
        Path.Combine(RepositoryRoot.Find(), "BenchmarkDotNet.Artifacts", target.Moniker);

    public static IConfig Create(BenchmarkTarget target, bool attribute)
    {
        var config = ManualConfig.Create(DefaultConfig.Instance)
            .AddJob(target.Job)
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddColumn(CategoriesColumn.Default)
            .AddExporter(MarkdownExporter.GitHub, JsonExporter.Full)
            .WithArtifactsPath(ArtifactsPath(target));

        return attribute ? config.AddDiagnoser(new EventPipeProfiler(EventPipeProfile.GcVerbose)) : config;
    }
}
