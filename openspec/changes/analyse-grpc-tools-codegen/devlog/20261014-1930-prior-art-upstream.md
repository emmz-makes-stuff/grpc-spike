# 5.4 done: prior-art and upstream surveys

<!-- Section: 5 · Tasks: 5.4 · Base: 996f6d3 -->

- **How the research was done.** Two background research agents gathered it with `gh` (unsandboxed), NuGet and
  READMEs. Each agent was told to cite only URLs it had opened.
  - I spot-checked protobuf #1700 (merged 2016-06-21), #29131 (open since 2026-08-11) and #7663 (closed
    2021-10-14). All three match.
  - Nothing was measured: the alternatives' allocation claims are their own.
- **Prior art: nothing is drop-in at either level.**
  - Every wire-compatible alternative (protobuf-net, LightProto, Protobuf.Generator, Lagrange.Proto) gives up the
    Google.Protobuf API and replaces `<Protobuf Include>`.
  - **Build hook for 5.5.** Grpc.Tools' `AdditionalProtocArguments` (or `PROTOBUF_PROTOC`/`GRPC_PROTOC_PLUGIN`) can
    add a protoc plugin without consumers changing their csproj. Nothing can turn off `--csharp_out`, though.
- **Upstream.** Every finding has a row, or "none found" with the searches tried. Highlights:
  - **M-16:** open PR protobuf #29131, which the maintainers invited.
  - **M-10, M-20, M-22:** earlier PRs died.
  - **M-05:** jskeet's own PR was never reviewed.
  - **M-03:** the ordering guarantee was removed from the docs in 2016.
- **Receptiveness.**
  - **protobuf C#:** small, clear fixes land fast, while large performance work stalls. jskeet wants evidence from a
    real application, and opt-in changes.
  - **grpc-dotnet:** effectively one reviewer, who does merge targeted performance PRs that come with numbers.
- **The `-0.0` observation:** no upstream issue exists. The report says it is worth filing. Filing it is Emmz's call
  and outward-facing, so it has not been done.

## Next

- 5.5: separate-tool assessment at both levels (API only; API plus build), stating the savings available and the
  rough effort at each. Use the Findings tiers and the build hook above.
- Then 5.6: the summary grid (shape × operation, current bytes, Tier A/B savings), checked against the exports.
- Deferred: squash the §5 WIP commits (86e40c8 is pushed) into one §5 commit (force-push: ask Emmz).
