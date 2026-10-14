# Stopped at 5.1: `StreamAndMergeBenchmarks` needs the AWS run

<!-- Section: 5 · Tasks: 5.1 built, run pending · Base: 996f6d3 -->

This note adds to `20261014-1414-decision-stream-merge-benchmarks.md`.

## Done this session

- **`StreamAndMergeBenchmarks` built** (52 samples × `ParseStream`, `WriteToStream`, `MergeBytesIntoPopulated`).
  The operations are on `MessageOperations<T>`. A quick local pass (Mac, 1 iteration) gave `BENCH_EXIT:0`, 156 rows,
  every row with allocated bytes. Those exports were deleted: they must come from AWS.
- **Bug found and fixed (Emmz: fix it in 5.1).** `ResultsExport` copied every `*-report-*` file in BDN's results
  folder, so a filtered run on the Mac copied stale reports (including the profiler-affected ones from the trace run
  below) over the committed results. They were restored with `git restore`; the committed AWS results were never
  wrong, because that run was on a fresh clone. It now copies only `<type>-report-{github.md,full.json}` for the
  types that ran, and throws if one is missing. Checked: a filtered marshaller run changed only the marshaller files
  and `environment.md`. The README now says a filtered run always rewrites `environment.md`.
- **Emmz chose a WIP commit that I push.** It is amended into the §5 commit at the end, which needs a force-push, so
  ask her first.
- Gates: `BUILD_EXIT:0`, `TEST_EXIT:0` (88 tests), `FORMAT_EXIT:0`, `GENERATED_EXIT:0`, `VALIDATE_EXIT:0`,
  `GATES_EXIT:0`.

## Evidence gathered for 5.2 (not in the repo: keep)

- **Attribution traces (Mac, net10, `ATTRIBUTE_EXIT:0`, 169 summaries)** for `MessageBenchmarks` ParseSpan, Clone and
  WriteToSegmentedWriter (all samples), plus `UnknownFieldBenchmarks`, `MarshallerBenchmarks` and
  `EndToEndBenchmarks`. They are in `BenchmarkDotNet.Artifacts/net10.0/**/*.summary.md`, which is gitignored and still
  on disk. Rerun with `make attribute FILTER=…` if they're gone.
- What the committed numbers plus the traces show. The report text is still to write. B = bytes per op, from
  `results/net10.0`, and net11 is identical for every message row:
  - **M-01/M-02 confirmed:** a 24 B `Int32` box per parse, clone or merge of a scalar oneof (e.g. ParseSpan
    `Oneof/populated-int32` 64 vs `Oneof/empty` 40; the trace shows `System.Int32` 37.5 %).
  - **M-03 confirmed:** `Maps/populated` ParseSpan is 24,552 B. In the trace, Dictionary `Entry[]` is about 37 %,
    `LinkedListNode` about 27 % and buckets `Int32[]` 7.6 %. Strings are 22 %.
  - **M-04/M-24 confirmed:** New `Maps` 504 B (152 B per empty map field), New `Repeated` 264 B (32 B per empty
    `RepeatedField`).
  - **M-25 confirmed:** Clone `Maps/empty` 960 = 504 + 456 discarded. Clone `Repeated/empty` 456 = 264 + 192
    discarded.
  - **M-05 confirmed:** 56 B boxed `LinkedList` enumerator per non-empty map per write (WriteToSpan `Maps/*` 168 B for
    3 maps; it shows in the trace).
  - **M-10 confirmed:** WriteToSegmentedWriter `Strings/populated` 16,760 B, `Maps/populated` 1,096 B. `Byte[]`
    dominates the traces.
  - **M-11 confirmed:** `Bytes/populated` ParseSpan 16,552 = exactly two copies + two `ByteString` + message.
  - **M-12 confirmed:** `Optional/edge-defaults-present` +56 B (`byte[0]` 24 + `ByteString` 32). `Wrappers` don't hit
    it: an empty wrapper is written as an empty sub-message, so the inner field is never read.
  - **M-13:** linear, 88 B per level (Node 48 + label string 40). Inherent.
  - **M-16 confirmed:** `Repeated/populated` ParseSpan 8,856 B. The arithmetic (checked against the trace:
    `Double[]` 33.4 %, `Int32[]` 24.8 %) gives about 2,688 B of discarded growth arrays plus about 640 B of slack,
    roughly 38 %. The packed double is pre-sized.
  - **M-18 confirmed (CPU):** CalculateSize at Node depth 1/3/8 is 13/40/108 ns (linear). WriteToSpan is 27/123/585 ns
    (quadratic). At depth 8, about 65 % of the write is re-sizing.
  - **M-20 confirmed:** ParseByteArray − ParseSpan = 168 B on every sample (`CodedInputStream`).
  - **M-22 confirmed:** `ToByteArray` = array + 64 B `CodedOutputStream` (empty messages 88 B).
  - **M-26 confirmed:** `UnknownFieldBenchmarks.ParseV2AsV1` is 1,376 B (UnknownFieldSet, Dictionary, UnknownField,
    lists).
  - **M-27 rejected:** `ReserializeUnknownsAsV1` is 136 B, and the trace shows only `Byte[]` and `CodedOutputStream`.
    No boxed Dictionary enumerator. Cause not investigated: .NET 10 may stack-allocate it.
  - **M-28/M-29 confirmed no site:** `Wrappers`/`Optional` parse is the message, plus strings and bytes.
  - **Hypothesis 8 rejected:** Marshaller Serialize 0 B, Deserialize 104 B (the `Scalars` message, 99.9 % of the
    trace). End-to-end unary is 14,240 B, so the generated layer's share is 2 × 104 B ≈ 1.5 %. The top types are all
    grpc-dotnet, ASP.NET Core and HTTP. net11 end-to-end is +1.1 to 2.9 KB per call.
  - The end-to-end rows have a few hundred bytes of noise (Unary 1 vs 10 differ by 129 B, though `Messages` is
    ignored), because BDN counts every thread.
  - Sampled shares can mislead on Clone: the `Repeated/populated` Clone trace shows only `RepeatedField` types, no
    arrays. Use arithmetic on the exact Allocated column for Clone.
- **The quick local pass (Mac) already shows the 5.1 sites.** Cite the AWS numbers once they're in:
  - **M-21:** ParseStream = ParseSpan + 4,288 B (4 KiB buffer + `CodedInputStream`).
  - **M-08:** `Strings/populated` ParseStream 70,880, i.e. about 33 KB of temporary arrays beyond that.
  - **M-23:** WriteToStream is 4,184 B.
  - **M-14:** `Oneof/populated-message` MergeBytesIntoPopulated 160 vs Clone 80. That is 40 B extra
    (`OneofPayload`) beyond the label string.

## The question for Emmz: run 5.1 on the AWS instance

Recipe, on the instance (about 20–30 min per runtime on the t3.medium):

```sh
cd ~/grpc-spike
git pull
tmux new -s bench          # or: tmux attach -t bench
make bench FILTER='*StreamAndMergeBenchmarks*' 2>&1 | tee ~/bench-stream.log
tail -1 ~/bench-stream.log
ls docs/analysis/results/net10.0 docs/analysis/results/net11.0
```

Then, on the Mac, from the repo root:

```sh
scp -i ~/.ssh/emmz-aws.pem -r ubuntu@18.169.131.244:grpc-spike/docs/analysis/results docs/analysis/
```

- **On PASS** (`BENCH_EXIT:0`, and each runtime has the two `StreamAndMergeBenchmarks` exports):
  - Check that `git status` shows the two new files per runtime, plus `environment.md` changing only if the machine
    line differs. Nothing else should change.
  - Each JSON should have 156 rows, every one with allocated bytes.
  - Then tick 5.1 and carry on with 5.2.
- **On FAIL:** read the log tail, fix, and give a narrowed recipe (`BENCH_TARGETS=…`).

## Next

- Waiting on Emmz's AWS run (above).
- Then 5.2: rewrite `docs/analysis/report.md` from provisional into the findings table, using the evidence above
  (cite the committed JSON by benchmark FullName).
- Deferred: amend the WIP commit into the single §5 commit (force-push: ask Emmz).
