using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// The runtime a run measures (design D1: .NET 10 primary, .NET 11 comparison). BenchmarkDotNet 0.15.8 has no
/// .NET 11 moniker, so net11.0 gets a custom csproj toolchain only; a custom runtime trips BenchmarkDotNet's SDK
/// validator. The host process stays on net10.0. Both use ShortRun (§4 decision): allocations are exact regardless,
/// and timings are secondary (D8).
/// </summary>
public sealed record BenchmarkTarget(string Moniker, Job Job)
{
    public const string Option = "--target";

    public static BenchmarkTarget Net10 { get; } = new("net10.0", Job.ShortRun.WithRuntime(CoreRuntime.Core10_0));

    public static BenchmarkTarget Net11 { get; } = new(
        "net11.0",
        Job.ShortRun.WithToolchain(CsProjCoreToolchain.From(new NetCoreAppSettings("net11.0", null, ".NET 11.0"))));

    public static IReadOnlyList<BenchmarkTarget> All { get; } = [Net10, Net11];

    /// <summary>Takes <c>--target &lt;moniker&gt;</c> out of the arguments; the rest are BenchmarkDotNet's.</summary>
    public static (BenchmarkTarget Target, string[] BenchmarkArgs) Parse(string[] args)
    {
        var index = Array.IndexOf(args, Option);
        if (index < 0 || index == args.Length - 1)
        {
            throw new ArgumentException($"Pass {Option} <{string.Join('|', All.Select(t => t.Moniker))}>.", nameof(args));
        }

        var moniker = args[index + 1];
        var target = All.SingleOrDefault(t => t.Moniker == moniker)
            ?? throw new ArgumentException($"Unknown {Option} '{moniker}'. Expected one of: {string.Join(", ", All.Select(t => t.Moniker))}.", nameof(args));

        return (target, [.. args[..index], .. args[(index + 2)..]]);
    }
}
