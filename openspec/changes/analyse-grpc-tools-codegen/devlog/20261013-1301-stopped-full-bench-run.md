# Stopped at 4.7: the full benchmark run needs a quiet machine

<!-- Section: 4 · Tasks: 4.1–4.6 built and verified, 4.7 pending · Base: 303be6f -->

§4 is built up to 4.6 and **left uncommitted on purpose** (CLAUDE.md stop rule). Tasks 4.1–4.6 are
unticked, although each one's verify was met in this session (details below). 4.7 needs timings from a
quiet machine, so Emmz runs it.

## Emmz's answers this session

- Runtimes: **one runtime per run.** `--target net10.0|net11.0`; the host is always net10.0. BDN 0.15.8 has
  no net11 moniker. net11.0 uses a **toolchain-only** job (`CsProjCoreToolchain` +
  `NetCoreAppSettings("net11.0")`). Adding `CoreRuntime.CreateForNewVersion` crashed BDN's
  `DotNetSdkValidator` (`GetRuntimeVersion not implemented for NotRecognized`), so it was dropped. The child
  reports `.NET 11.0.0 (11.0.0-rc.1…)`.
- Subject versions: **sidecar `environment.md`** per runtime. It holds every `Directory.Packages.props` pin
  (stamped into the assembly by the `StampPackagePins` target), the loaded assembly versions, the runtime
  as measured, and the Apple Silicon timing label. It is needed because the JSON's `HostEnvironmentInfo`
  is the net10 **host**, even in `net11.0/`, and the JSON has no child runtime at all.
- Job: **ShortRun for the whole suite.** Allocations are exact either way; timings are secondary (D8).
- Samples: **all 52** (`Samples.All`).
- End-to-end streams: **`[Params(1, 10)]` messages.** Unary ignores the param, so its rows repeat.
- Reference shapes: Scalars, Strings, Bytes, Repeated(+Item), Maps(+MapValue), Node, Optional. **No
  oneof or wrappers**: protobuf-net models them differently, so the comparison isn't like-for-like.
- Attribution summaries: **built-in summariser** (`AllocationAttribution`, TraceEvent 3.1.21, which BDN
  already depends on, so no new dependency). Wrapped as `make attribute FILTER=…`.
- Rosetta 2 dropped, as agreed before §4: the row is gone from `docs/dependencies.md` and the check from
  `make deps`. `DEPS_EXIT:0` afterwards.

## Other decisions

- Message ops: `New`, `ParseByteArray`/`ParseSpan` (M-20), `ToByteArray`, `WriteToSpan`,
  `WriteToSegmentedWriter` (32-byte segments, M-10), `CalculateSize`, `Clone` (M-25),
  `MergeIntoNew`/`MergeIntoPopulated` (M-02/M-14). The merge rows include a `New` or `Clone`, which the
  report subtracts. `UnknownFieldBenchmarks` covers M-26/M-27.
- The marshaller is captured through `CorpusService.BindService` with a capturing `ServiceBinderBase`, because
  the generated field is private.
- Reference data is protobuf-net deserializing the generated code's bytes. `ReferenceEquivalenceTests`
  compares values field by field. A bytes-only round-trip missed a deliberately wrong `ZigZag`; the
  value check caught it (3 failures), then passed again after the revert.
- `make bench`: a `FILTER` variable is added, because a second `--filter` in `ARGS` is a BDN argument error.
  Program exits 1 if nothing ran or a benchmark failed, because BDN itself exits 0.
- Attributed runs don't copy exports into `results/` (profiler overhead).

## Verified this session

- Quick passes with 1–3 iterations. All 520 message rows and 3 unknown-field rows ran on net10. Marshaller
  and end-to-end rows ran on **both** runtimes, so TestHost 10.0.12 under net11.0 works and that watch
  item is closed. All 120 reference rows ran on net10. Every row has allocated bytes. Those exports were
  deleted, because they must not be committed.
- `make attribute FILTER='*UnknownFieldBenchmarks.ParseV2AsV1*'` gave `ATTRIBUTE_EXIT:0`. It named types and
  sites (`UnknownFieldSet.GetOrAddField` and its Dictionary growth dominate). Without a FILTER it gives
  `ATTRIBUTE_EXIT:2`.
- `make gates`: `BUILD_EXIT:0`, `TEST_EXIT:0` (88 tests), `FORMAT_EXIT:0`, `GENERATED_EXIT:0`,
  `VALIDATE_EXIT:0`, `GATES_EXIT:0`.

## The question for Emmz: run 4.7 on a quiet machine

Recipe (about 653 benchmarks per runtime, roughly 2–3 h in all):

```sh
cd ~/github/emmz/grpc-spike
# Quiet machine: on mains power, other apps closed, no builds/indexing/backups running.
make bench 2>&1 | tee "$TMPDIR/bench-full.log"
# Then paste back:
tail -1 "$TMPDIR/bench-full.log"
ls docs/analysis/results/net10.0 docs/analysis/results/net11.0
```

`// Failed to set up high priority (Permission denied)` lines are expected. Don't run it under `sudo`.

- **On PASS** (`BENCH_EXIT:0` and both folders hold 5 classes × github.md + full.json, plus
  `environment.md`): check each JSON's benchmark count (MessageBenchmarks 520, UnknownField 3,
  Marshaller 2, EndToEnd 8, Reference 120) on both runtimes. Then tick 4.1–4.7, write the §4 done note
  (pointing back here), make the one §4 commit with the results, and hand off.
- **On FAIL:** read the tail of the log, fix, and give Emmz a narrowed rerun recipe
  (`make bench BENCH_TARGETS=… FILTER=…`).

## Next

- Waiting on Emmz's 4.7 run. Follow PASS/FAIL above.
- Deferred nit for the §4 done note or §5: `design.md` still says Grpc.Tools is x64-only, with Rosetta as a
  logged dependency (in "checked facts" and Risks). It's stale since Grpc.Tools 2.84.0. Ask Emmz whether to
  amend the design artifact.
- Then §5, starting at 5.1. The attribution how-to is in `docs/analysis/README.md`.
