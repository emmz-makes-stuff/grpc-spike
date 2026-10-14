# Allocation tests: M-25 and M-27 hardened against slow tier-up

<!-- Section: 5 · Tasks: 5.3 (follow-up) · Base: 996f6d3 -->

This adds to `20261014-1923-allocation-tests.md` and to the section-5-done note.

- **The risk** (raised in a side-agent note Emmz forwarded). M-25's map clone and M-27 reach the committed bytes only
  after the JIT's tiered PGO removes their boxed enumerators (2 × 56 B). On a busy machine a fixed 500 ms warm-up might
  not be enough, so they could fail at random.
- **Tried: tiered compilation off** (`DOTNET_TieredCompilation=0`). Both still measured 112 B over, so the
  enumerators are removed only with PGO, not by full optimisation alone. No deterministic switch is available.
- **Fix.** `AllocatedOnceOptimised(expected, op)` re-measures until it sees the expected bytes or 10 s pass. Only
  those three asserts use it. A real change still fails, after the deadline.
- **Checked.**
  - With `DOTNET_TC_CallCountingDelayMs=500` (5× the default tier-up delay), 19/19 pass.
  - With 2000 ms, M-25 and M-27 still fail, even M-27 run alone. The cause was not investigated.
  - **Conclusion:** sturdier, not guaranteed. If M-25 or M-27 ever fails alone, suspect tier-up before the code.
- Gates: `BUILD_EXIT:0`, `TEST_EXIT:0` (107 tests), `FORMAT_EXIT:0`, `GENERATED_EXIT:0`, `VALIDATE_EXIT:0`,
  `GATES_EXIT:0`. Amended into the local §5 commit.

## Next

- Unchanged from the section-5-done note:
  - §6.1 is Emmz's go/no-go decision (ADR-0003).
  - The squashed §5 commit needs her yes to force-push over `86e40c8`.
