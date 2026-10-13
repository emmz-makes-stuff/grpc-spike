# Decision: run 4.7 on an AWS Ubuntu x86_64 instance; WIP push

<!-- Section: 4 · Tasks: 4.7 pending · Base: 303be6f -->

This note adds to `20261013-1301-stopped-full-bench-run.md`; it doesn't replace it.

- **Emmz:** the quiet-machine run for 4.7 happens on an **AWS Ubuntu instance (x86_64)**, at
  `ubuntu@18.169.131.244` with key `~/.ssh/emmz-aws.pem`. Not on the Mac.
  - This supersedes design D8's fixed label "Apple Silicon (arm64), macOS".
  - It also supersedes the non-goal "Linux/x64 timing runs". Allocations remain the primary evidence.
- **Label is now derived (Emmz chose it).** `environment.md` writes "Timings are specific to this machine
  (<CPU>, <arch>, <OS>)", using BDN's processor name and `RuntimeInformation`. It is no longer
  hard-coded. `docs/analysis/README.md` now says each `environment.md` names its machine. Checked on the
  Mac: "Apple M3 Max, Arm64, macOS 26.6.2".
- **Transport (Emmz chose it): a WIP commit, pushed.** §4 so far (4.1–4.6, unticked) is committed as `wip(…)` on
  `change/analyse-grpc-tools-codegen` and pushed to origin, so the instance can clone it.
  - This bends the "leave a stop uncommitted" rule, on purpose.
  - When the results come back, **amend/squash the WIP commit into the single §4 section commit**. That
    needs a force-push of the branch, so ask Emmz first.
- Gates after the label change: `BUILD_EXIT:0`, `TEST_EXIT:0`, `FORMAT_EXIT:0`, `GENERATED_EXIT:0`,
  `VALIDATE_EXIT:0`, `GATES_EXIT:0`.

## Next

- Waiting on Emmz to run `make bench` on the instance (inside tmux), then copy
  `docs/analysis/results/` back to the Mac. Then follow the PASS/FAIL steps in the 1301 stop note.
- In the §4 done note, flag for `design.md`: D8's label, the Linux/x64 non-goal, and the stale
  Rosetta/x64-protoc facts. Ask Emmz whether to amend them.
