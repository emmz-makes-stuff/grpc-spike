# grpc-spike

## OpenSpec workflow

<!-- emmz-setup: 0.1.2 -->

Changes are tracked with OpenSpec. Build them with `/opsx:apply`, following this loop on top of it.
Notes go in the change's `devlog/` folder via `/emmz:devlog`.

### Gates

Gates run through the `Makefile`. A gate passed only if you saw its `LABEL_EXIT:0`. Quote exit lines,
never paraphrase them.

- `make build`: `dotnet build GrpcSpike.slnx -c Release`. Analyzers run in the build, with warnings as
  errors.
- `make test`: `dotnet test --solution GrpcSpike.slnx -c Release` (xUnit v3 on Microsoft.Testing.Platform).
- `make format`: `dotnet format GrpcSpike.slnx --verify-no-changes`. `src/Corpus/Generated/` is excluded.
- `make generated`: checks that the committed generated code in `src/Corpus/Generated/` matches what the
  build just produced. Run it after `make build`.
- `make validate`: `openspec validate --strict` on every active change.
- `make gates`: all of the above.

`make bench` runs the BenchmarkDotNet suite. It is **not** a gate, because it is long-running.

`make deps` is separate: it checks that every tool and login in `docs/dependencies.md` is present.

### Starting a change

1. Ask which change to build, even if there is only one (`make changes`).
2. Find the resume point: the newest devlog note's `## Next` and the first unticked task in `tasks.md`.
   In a fresh session, `/emmz:pickup` does this and steps 3–5.
3. Run `make deps` and look for `DEPS_EXIT:0`. If it's missing, stop: offer to install what's missing,
   and hand any login to the user as `! <cmd>`. Never try to authenticate yourself.
4. Check for a clean working tree (unless resuming a recorded stop) and a green `make validate`, on
   branch `change/<change-name>`.
5. Read `proposal.md`, `design.md`, and the specs the change touches.

A task that needs a tool, SDK, service or login not in `docs/dependencies.md` is a stop: ask before
adding it. Once agreed, add its row there and its check to `make deps` in the same section commit, and
say why in a devlog note.

### Each section (`## N.` in `tasks.md`), in order

1. Note the base commit (`git rev-parse --short HEAD`). For larger sections, put it in a start note.
2. Implement the section's tasks, with tests. Write a devlog note for any non-obvious decision.
3. **Self-review** `git diff <base>` plus untracked files, against the spec requirements rather than
   just the tasks. Look for:
   - missing requirements;
   - untested behaviour;
   - swallowed errors;
   - dead or stub code;
   - contracts that drifted from earlier sections;
   - violations of `design.md ## Decisions` and the ADRs in docs/adrs/.

   Fix what you find.
4. Run `make gates`; everything must be green. A section may land red only as a deliberate, stated call
   (for example, a test that a later section turns green). Record it in the devlog note and the commit
   body.
5. Tick the section's tasks and write the section-done devlog note. Assume the context will be cleared
   before the next section, so the note must be enough to resume from alone.
   - Its `## Next` names the next section and its first task (or the whole-change review, after the
     last section).
   - It also records anything this session knows that the repo doesn't: the user's answers, gotchas
     you hit, deferred nits, decisions in flight.

   Then make **one commit** for the section:
   ```
   feat(<change-name>): <section summary> (§N)

   - N.1 <task summary>
   - N.2 <task summary>
   ```
6. **Hand off.** Run `/emmz:handoff` to check that a cold session can resume from the repo alone. Then
   end the turn by asking whether the user wants to `/clear` the context or restart `claude` before the
   next step. If they'd rather keep going, continue in this session.

### Finishing a change

1. Self-review the whole change with `git diff <first-section-base>..HEAD`. Look for duplicated
   abstractions, leftover scaffolding, cross-section drift, and whether the spec is genuinely satisfied.
2. Write the change-done devlog note and report to the user. Then **offer** to run the `code-review`
   skill on the change branch before archiving, and run it if they say yes.
3. Fixes from either review don't land as loose commits. Add them to `tasks.md` as one or more new
   sections (`## N+1. Review fixes: <theme>`) and agree them with the user. Build them with the section
   loop above, hand-off included, then return to step 2.
4. Once the review is done or declined and no fix sections are open, **offer** `/opsx:archive`, and
   wait for a yes.

### Stop and ask

Stop when:
- a requirement is ambiguous or contradicts the spec or design;
- scope would change;
- a task needs human verification: the go/no-go decision in ADR-0003, judgement calls on whether a
  finding is "worth it", or benchmark runs whose timings must come from a quiet machine;
- gates won't go green after two honest attempts.

To stop:
1. Leave work uncommitted and tasks unticked.
2. Write a `stopped-…` devlog note stating exactly what blocked and the question.
3. Send a push notification if the tool is available.
4. End the turn with the question. For human verification, include a copy-pasteable recipe.

Never decide a product question yourself and flag it afterwards.

Before the user ends a session or clears the context at any other point, run `/emmz:handoff`. Resume
with `/emmz:pickup`.
