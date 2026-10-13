# grpc-spike — gate targets.
#
# Every gate prints its own exit code as `LABEL_EXIT:<n>` and exits with it, so a
# result can be quoted rather than inferred from output that may look clean.
# `gates` runs the whole set with -k, so one run reports every failure.

SHELL := /bin/bash

CHANGES := $(notdir $(patsubst %/,%,$(filter-out %/archive/,$(wildcard openspec/changes/*/))))

.PHONY: build test format generated validate changes gates deps bench attribute

build:
	@dotnet build GrpcSpike.slnx -c Release; code=$$?; echo "BUILD_EXIT:$$code"; exit $$code

test:
	@dotnet test --solution GrpcSpike.slnx -c Release; code=$$?; echo "TEST_EXIT:$$code"; exit $$code

format:
	@dotnet format GrpcSpike.slnx --verify-no-changes; code=$$?; echo "FORMAT_EXIT:$$code"; exit $$code

# Committed generated code must match what the pinned Grpc.Tools produces (run after build).
generated:
	@git diff --exit-code -- src/Corpus/Generated && test -z "$$(git ls-files --others --exclude-standard -- src/Corpus/Generated)"; \
	code=$$?; echo "GENERATED_EXIT:$$code"; exit $$code

# Validates every active change. No active change is a failure, not a silent pass.
validate:
	@fail=0; \
	if [ -z "$(CHANGES)" ]; then \
		echo "no active change found under openspec/changes/"; fail=1; \
	else \
		for c in $(CHANGES); do openspec validate $$c --strict || fail=1; done; \
	fi; \
	echo "VALIDATE_EXIT:$$fail"; exit $$fail

changes:
	@echo "$(CHANGES)"

gates:
	@$(MAKE) --no-print-directory -k build test format generated validate; code=$$?; \
	echo "GATES_EXIT:$$code"; exit $$code

# Not a gate: long-running. One run per runtime (net10.0 primary, net11.0 comparison); the host is always
# net10.0 and BenchmarkDotNet starts the runtime under test. Exports land in docs/analysis/results/<runtime>/.
# Narrow the run with FILTER (make bench FILTER='*Oneof*'), pick one runtime with BENCH_TARGETS=net11.0, and
# pass other BenchmarkDotNet args via ARGS.
BENCH_TARGETS ?= net10.0 net11.0
FILTER ?= *
bench:
	@fail=0; \
	for t in $(BENCH_TARGETS); do \
		dotnet run -c Release --framework net10.0 --project bench/Benchmarks -- --target $$t --filter '$(FILTER)' $(ARGS) || fail=1; \
	done; \
	echo "BENCH_EXIT:$$fail"; exit $$fail

# Not a gate: allocation attribution (task 4.6) for the benchmarks FILTER selects, on one runtime
# (BENCH_TARGET, default net10.0). Writes and prints a <trace>.summary.md beside each GcVerbose trace under
# BenchmarkDotNet.Artifacts/; see docs/analysis/README.md. Refuses the whole suite: one trace per benchmark.
BENCH_TARGET ?= net10.0
attribute:
	@if [ "$(FILTER)" = "*" ]; then echo "attribute needs FILTER='<benchmarks>'"; echo "ATTRIBUTE_EXIT:2"; exit 2; fi; \
	dotnet run -c Release --framework net10.0 --project bench/Benchmarks -- --target $(BENCH_TARGET) --attribute --filter '$(FILTER)' $(ARGS); \
	code=$$?; echo "ATTRIBUTE_EXIT:$$code"; exit $$code

# Every tool and login in docs/dependencies.md — keep the two in step. Not part of
# `gates`: it checks the machine, not the code, and auth checks may need the network.
deps:
	@fail=0; \
	dotnet --version >/dev/null 2>&1 || { echo "MISSING dotnet — brew install --cask dotnet-sdk@preview"; fail=1; }; \
	dotnet --version 2>/dev/null | grep -q '^11\.' || { echo "WRONG VERSION dotnet — need 11.0 SDK (global.json)"; fail=1; }; \
	dotnet --list-runtimes 2>/dev/null | grep -q 'Microsoft.NETCore.App 10\.' || { echo "MISSING .NET 10 runtime — brew install --cask dotnet-sdk"; fail=1; }; \
	openspec --version >/dev/null 2>&1 || { echo "MISSING openspec — npm i -g @fission-ai/openspec"; fail=1; }; \
	git --version >/dev/null 2>&1 || { echo "MISSING git — brew install git"; fail=1; }; \
	gh --version >/dev/null 2>&1 || { echo "MISSING gh — brew install gh"; fail=1; }; \
	gh auth status >/dev/null 2>&1 || { echo "NOT AUTHENTICATED gh — gh auth login"; fail=1; }; \
	echo "DEPS_EXIT:$$fail"; exit $$fail
