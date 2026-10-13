# grpc-spike — gate targets.
#
# Every gate prints its own exit code as `LABEL_EXIT:<n>` and exits with it, so a
# result can be quoted rather than inferred from output that may look clean.
# `gates` runs the whole set with -k, so one run reports every failure.

SHELL := /bin/bash

CHANGES := $(notdir $(patsubst %/,%,$(filter-out %/archive/,$(wildcard openspec/changes/*/))))

.PHONY: build test format generated validate changes gates deps bench

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

# Not a gate: long-running. Pass extra BenchmarkDotNet args via ARGS, e.g. make bench ARGS="--filter '*Oneof*'".
bench:
	@dotnet run -c Release --project bench/Benchmarks -- --filter '*' $(ARGS); code=$$?; echo "BENCH_EXIT:$$code"; exit $$code

# Every tool and login in docs/dependencies.md — keep the two in step. Not part of
# `gates`: it checks the machine, not the code, and auth checks may need the network.
deps:
	@fail=0; \
	dotnet --version >/dev/null 2>&1 || { echo "MISSING dotnet — brew install --cask dotnet-sdk@preview"; fail=1; }; \
	dotnet --version 2>/dev/null | grep -q '^11\.' || { echo "WRONG VERSION dotnet — need 11.0 SDK (global.json)"; fail=1; }; \
	dotnet --list-runtimes 2>/dev/null | grep -q 'Microsoft.NETCore.App 10\.' || { echo "MISSING .NET 10 runtime — brew install --cask dotnet-sdk"; fail=1; }; \
	arch -x86_64 /usr/bin/true >/dev/null 2>&1 || { echo "MISSING Rosetta 2 — softwareupdate --install-rosetta --agree-to-license"; fail=1; }; \
	openspec --version >/dev/null 2>&1 || { echo "MISSING openspec — npm i -g @fission-ai/openspec"; fail=1; }; \
	git --version >/dev/null 2>&1 || { echo "MISSING git — brew install git"; fail=1; }; \
	gh --version >/dev/null 2>&1 || { echo "MISSING gh — brew install gh"; fail=1; }; \
	gh auth status >/dev/null 2>&1 || { echo "NOT AUTHENTICATED gh — gh auth login"; fail=1; }; \
	echo "DEPS_EXIT:$$fail"; exit $$fail
