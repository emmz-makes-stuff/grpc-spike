# Decision: benchmark the four unmeasured sites, as new task 5.1

<!-- Section: 5 · Tasks: new 5.1 (old 5.1–5.5 are now 5.2–5.6) · Base: 996f6d3 -->

- **The gap.** Four §3 sites have no committed benchmark: M-08 (string decode when parsing from a `Stream`),
  M-14 (a oneof message parsed into a member that is already set), M-21 (`ParseFrom(Stream)`) and M-23
  (`WriteTo(Stream)`). The spec says a finding cites a committed benchmark result. None of the four is on the gRPC
  path, which uses `ReadOnlySequence`/`IBufferWriter`.
- **Emmz chose:** add benchmarks and rerun them on the AWS instance, so every committed number comes from one
  machine. She rejected recording them as "unmeasured", and rejected running them on the Mac.
- **Shape (my call).** A new class, `StreamAndMergeBenchmarks`: every sample × `ParseStream`, `WriteToStream` and
  `MergeBytesIntoPopulated` (Clone, then `MergeFrom(ReadOnlySpan)` of the sample's own bytes; subtract the
  `MessageBenchmarks` Clone row). The operations live on `MessageOperations<T>` beside the others. It is a separate
  class, so the committed `MessageBenchmarks` rows from 4.7 are not rerun. Both streams are reused, so only the
  protobuf calls allocate.
- **Expected rows:** 52 × 3 = 156 per runtime.
- **Tasks renumbered:** new 5.1 is this. Old 5.1 (confirm/reject) is now 5.2, and so on to 5.6. Earlier notes that
  say "5.1" mean today's 5.2.

## Next

- Finish 5.1: build, quick local pass, gates. Then it's a stop for Emmz's AWS run (push needed first: ask).
