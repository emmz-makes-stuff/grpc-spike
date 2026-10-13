using BenchmarkDotNet.Attributes;
using Grpc.Net.Client;
using GrpcSpike.Corpus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// Whole in-process calls through TestHost (task 4.4, design D5), with the message <see cref="MarshallerBenchmarks"/>
/// marshals. Allocations cover client and server. The service is a singleton (report S-31), unary calls use
/// <c>UnaryAsync</c> (S-29) and compression stays off (S-36). <see cref="Messages"/> at 1 and 10 separates the
/// per-call cost from the per-message cost.
/// </summary>
public class EndToEndBenchmarks
{
    private WebApplication app = null!;
    private GrpcChannel channel = null!;
    private CorpusService.CorpusServiceClient client = null!;
    private Scalars request = null!;

    [Params(1, 10)]
    public int Messages { get; set; }

    [GlobalSetup]
    public async Task Setup()
    {
        var builder = WebApplication.CreateSlimBuilder();
        builder.WebHost.UseTestServer();
        builder.Logging.ClearProviders();
        builder.Services.AddGrpc();
        builder.Services.AddSingleton<EchoService>();

        app = builder.Build();
        app.MapGrpcService<EchoService>();
        await app.StartAsync();

        var server = app.GetTestServer();
        channel = GrpcChannel.ForAddress(server.BaseAddress, new GrpcChannelOptions { HttpHandler = server.CreateHandler() });
        client = new CorpusService.CorpusServiceClient(channel);

        // Same message as the marshaller rows; only Int32Value changes, to carry the server-streaming length.
        request = StubSamples.Request.Clone();
        request.Int32Value = Messages;
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        channel.Dispose();
        await app.StopAsync();
        await app.DisposeAsync();
    }

    [Benchmark]
    public async Task<Scalars> Unary() => await client.UnaryAsync(request);

    [Benchmark]
    public async Task<int> ServerStreaming()
    {
        using var call = client.ServerStreaming(request);
        var received = 0;
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            received++;
        }

        return received;
    }

    [Benchmark]
    public async Task<Scalars> ClientStreaming()
    {
        using var call = client.ClientStreaming();
        for (var i = 0; i < Messages; i++)
        {
            await call.RequestStream.WriteAsync(request);
        }

        await call.RequestStream.CompleteAsync();
        return await call;
    }

    [Benchmark]
    public async Task<int> Bidi()
    {
        using var call = client.Bidi();
        var received = 0;
        for (var i = 0; i < Messages; i++)
        {
            await call.RequestStream.WriteAsync(request);
            if (await call.ResponseStream.MoveNext(CancellationToken.None))
            {
                received++;
            }
        }

        await call.RequestStream.CompleteAsync();
        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            received++;
        }

        return received;
    }
}
