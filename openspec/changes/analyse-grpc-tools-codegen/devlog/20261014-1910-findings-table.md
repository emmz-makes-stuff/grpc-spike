# 5.2 done: `report.md` is now the findings table

<!-- Section: 5 · Tasks: 5.2 · Base: 996f6d3 -->

- **Structure.** The top of `docs/analysis/report.md` now has, in order:
  - Subject and citations; Measurements (how to cite a row: its JSON `FullName` without `GrpcSpike.Benchmarks.`);
  - tiers;
  - Hypotheses, each with a status (1–7 confirmed, with 3 confirmed off the default path only and 4/5 mostly
    inherent; 8 rejected);
  - Findings: 20 rows, each with tier, hot/one-off, source, measured result and the avoidable bytes;
  - Sites not counted: rejected, inherent, no site, not isolated, one-off and off-path, each with its evidence.

  The §3 tables are now Appendix A and Appendix B, kept as the full citation record.
- **Verify.** A script check found every Findings row cites a source (repo path or pinned GitHub link) and a
  committed benchmark row.
- **Exact arithmetic.** These match the AWS bytes exactly:
  - M-04: `New(Maps/empty)` 504 = 48 + 3 × 152.
  - M-24: `New(Repeated/empty)` 264 = 72 + 6 × 32.
  - M-16: ParseSpan `Repeated/populated` 8,856 (2,688 B discarded + 640 B slack).
  - M-17: Clone `Repeated/populated` 4,984 (864 B slack; the source was built by `Add`).
  - M-25: Clone `Maps/empty` and `Repeated/empty`.
  - M-20/21/22/23: 168 / 4,288 / 64 / 4,184.
- **New this pass.**
  - **M-27 rejected exactly.** `ReserializeUnknownsAsV1` is 136 B, the same as `ToByteArray(Unknown.V2.Record/populated)`.
  - **M-26.** It costs 1,056 B per parse (the same bytes parsed by V2 cost 320) and 536 B per Clone.
  - **net11 message rows.** They are identical, except the 1 MB edge rows (noise) and
    `MergeIntoPopulated(Maps/populated)`, which is 56 B lower on net11. That is one enumerator's size, probably
    stack-allocated; not investigated.
  - **M-08.** The temporaries are about 2× the string bytes, but only about 1× for bytes. The mechanism was not
    traced, and the report says so.
- **My calls, for Emmz to check in review.**
  - M-06, M-09 and M-19 (second size passes, CPU) are "not isolated", not findings: no benchmark separates them.
  - S-20–S-35 is one aggregate row (Tier B, out of a generator's reach).
  - The report says nothing about whether a finding is worth it. That is left to the ADR.
- `VALIDATE_EXIT` below; the full gates run at the end of the section.

## Next

- 5.3: allocation-count tests for each confirmed finding, per design D6: `GC.GetAllocatedBytesForCurrentThread()`
  around a warmed operation, asserting the documented bytes. Ask Emmz to confirm the approach first (code change).
- Deferred: squash the §5 WIP commits into one §5 commit (force-push: ask Emmz).
