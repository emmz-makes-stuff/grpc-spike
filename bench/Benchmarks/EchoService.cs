using Grpc.Core;
using GrpcSpike.Corpus;

namespace GrpcSpike.Benchmarks;

/// <summary>
/// Echoes requests back, so a call's cost is the gRPC and generated-code path, not service logic. Server streaming
/// sends the request back <see cref="Scalars.Int32Value"/> times, so the request carries the stream length.
/// </summary>
public sealed class EchoService : CorpusService.CorpusServiceBase
{
    public override Task<Scalars> Unary(Scalars request, ServerCallContext context) => Task.FromResult(request);

    public override async Task ServerStreaming(Scalars request, IServerStreamWriter<Scalars> responseStream, ServerCallContext context)
    {
        for (var i = 0; i < request.Int32Value; i++)
        {
            await responseStream.WriteAsync(request);
        }
    }

    public override async Task<Scalars> ClientStreaming(IAsyncStreamReader<Scalars> requestStream, ServerCallContext context)
    {
        Scalars? last = null;
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            last = requestStream.Current;
        }

        return last ?? new Scalars();
    }

    public override async Task Bidi(IAsyncStreamReader<Scalars> requestStream, IServerStreamWriter<Scalars> responseStream, ServerCallContext context)
    {
        while (await requestStream.MoveNext(context.CancellationToken))
        {
            await responseStream.WriteAsync(requestStream.Current);
        }
    }
}
