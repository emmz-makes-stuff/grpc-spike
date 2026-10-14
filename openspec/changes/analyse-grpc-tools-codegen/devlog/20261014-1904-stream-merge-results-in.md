# 5.1 done: `StreamAndMergeBenchmarks` results are in from AWS

<!-- Section: 5 · Tasks: 5.1 · Base: 996f6d3 -->

This note answers `20261014-1459-stopped-stream-merge-aws-run.md`.

- **Emmz ran the recipe on the AWS instance:** `BENCH_EXIT:0`, then copied the results to the Mac with scp.
- **Checks (PASS):**
  - `git status` shows only the four new `StreamAndMergeBenchmarks` exports. `environment.md` is byte-identical on
    both runtimes, so the machine line matches 4.7.
  - Both JSONs: 156 rows, 156 with allocated bytes. The host CPU is the Intel Xeon Platinum 8259CL, as in 4.7.
  - net10 and net11 allocations are identical, except the 1 MB `*/edge` rows (under 30 B apart: LOH noise).
- **AWS numbers for the sites** (net10, B/op; they match the Mac quick pass exactly):
  - **M-21:** ParseStream − ParseSpan = 4,288 B on 48 of 52 samples (4 KiB buffer + `CodedInputStream`).
  - **M-08:** `Strings/populated` ParseStream 70,880 vs ParseSpan 33,496, i.e. 37,384 B over (33,096 beyond M-21).
    `Bytes/populated` is 37,528 vs 16,552 (+20,976).
  - **M-23:** WriteToStream is 4,184 B on 48 samples (`Strings/populated` 20,592, `Maps/*` 4,352).
  - **M-14:** `Oneof/populated-message` MergeBytesIntoPopulated 160 vs Clone 80.
- The results are committed locally as a WIP commit (not pushed). They will be folded into the single §5 commit.

## Next

- 5.2: rewrite `docs/analysis/report.md` from provisional into the findings table, using the evidence in the
  stopped note above plus these numbers (cite the committed JSON by benchmark FullName).
- Deferred: squash the §5 WIP commits into one §5 commit (force-push: ask Emmz).
