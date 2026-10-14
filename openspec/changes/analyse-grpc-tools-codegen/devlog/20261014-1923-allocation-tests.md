# 5.3 done: allocation-count tests pin every confirmed finding

<!-- Section: 5 · Tasks: 5.3 · Base: 996f6d3 -->

- **Emmz chose "Go, as design D6".** The tests are in `tests/Corpus.Tests/AllocationTests.cs`: 19 facts, one per
  confirmed finding (M-01–M-05, M-08, M-10, M-12, M-14, M-16, M-17, M-20–M-26), plus M-27 as a rejection test.
  - Each asserts the exact bytes `report.md` quotes, measured with `GC.GetAllocatedBytesForCurrentThread()`, as the
    minimum of 20 runs after warm-up.
  - M-18 is CPU only, so it has no allocation test.
  - Apple arm64 gives the same bytes as the AWS x86_64 run.
- **Gotcha: warm-up has to outlast tier-up.**
  - With 1,000 warm-up calls, M-25 and M-27 measured exactly 112 B (2 × 56) too much. Those calls finish inside
    the JIT's 100 ms call-counting delay, so the code measured was unoptimised.
  - The warm-up is now time-based (500 ms). Three runs in a row: 19/19 pass, about 19 s.
  - This also settles M-27's open cause. The two boxed `Dictionary` enumerators (`CalculateSize` and `WriteTo`)
    exist at tier 0, and the optimised JIT removes them. The report row now says so.
- **Report additions.**
  - A line pointing at the tests.
  - Advice to weigh each finding by whether it is on the gRPC path, from a side-agent note:
    - `Stream` and `byte[]` rows are off the path;
    - Tier A on the path is 24–152 B per operation, plus M-18's CPU cost;
    - the larger on-path costs are Tier B.
- **Gate gotcha.** A sandboxed and an unsandboxed build collided (MSB4014/MSB4223). `dotnet build-server shutdown`,
  then an unsandboxed `make gates`, fixed it.
- Gates: `BUILD_EXIT:0`, `TEST_EXIT:0` (107 tests), `FORMAT_EXIT:0`, `GENERATED_EXIT:0`, `VALIDATE_EXIT:0`,
  `GATES_EXIT:0`.

## Next

- 5.4: prior-art survey (alternative generators and serializers, with drop-in fit) and upstream survey
  (contribution receptiveness, plus linked issues and PRs per confirmed finding, or "none found"). Include the
  `-0.0` double round-trip observation.
- Then 5.5 (separate-tool assessment at both compatibility levels) and 5.6 (summary grid).
- Deferred: squash the §5 WIP commits (86e40c8 is pushed) into one §5 commit (force-push: ask Emmz).
