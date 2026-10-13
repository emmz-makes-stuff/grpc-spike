# Spec Delta

## Purpose

Turns the code reading and the measurements into a written, evidence-backed analysis, and records the
resulting decision on whether a more efficient alternative to the standard generated code is worth
pursuing, and how.

## ADDED Requirements

### Requirement: Findings cite evidence
Each finding SHALL identify an allocation or CPU cost in the generated code or the runtime it calls into.
It SHALL cite the committed source location and the benchmark result that measures it.

#### Scenario: Finding is traceable
- **WHEN** a reader follows a finding's citations
- **THEN** they reach the exact generated or runtime source location and a committed benchmark result showing its cost

### Requirement: Tier classification
Each finding SHALL be classified by what removing it requires:
- **Tier A:** changes to generated private members or method bodies only;
- **Tier B:** changes to the Protobuf runtime's internals without changing its public API;
- **Tier C:** a change to the public API.

#### Scenario: Tier C is recorded but not counted
- **WHEN** a finding is classified Tier C
- **THEN** it appears in the report, marked as excluded from the go/no-go criteria because it breaks drop-in compatibility

### Requirement: Hot-path versus one-off
Each finding SHALL state whether its cost happens on every message operation or call, or once, for
example per type or per process.

#### Scenario: One-off cost is distinguished
- **WHEN** a finding's cost happens only once per type or process
- **THEN** the report labels it one-off and does not present it as a per-operation saving

### Requirement: Hypotheses resolved
The report SHALL confirm or reject each hypothesis listed at the start of the analysis, with evidence:
- boxing of oneof scalars;
- per-entry overhead in maps;
- string decoding;
- bytes copying on parse;
- nested message instances;
- repeated-field growth;
- repeated size calculation on deep messages;
- headroom in the stub layer.

#### Scenario: Rejected hypothesis is shown
- **WHEN** the evidence shows a hypothesised cost does not exist or is negligible
- **THEN** the report states it is rejected and shows the measurement that rejects it

### Requirement: Prior-art and upstream survey
The report SHALL survey existing alternative generators and serializers, and assess each one's drop-in
compatibility. It SHALL also assess how the upstream projects receive contributions, including any
existing issues or pull requests about the costs found.

#### Scenario: Upstream history informs a finding
- **WHEN** an upstream issue or pull request already addresses a finding
- **THEN** the report links it and states its outcome

### Requirement: Drop-in scope assessed both ways
For the separate-tool route, the report SHALL assess two levels of compatibility separately: compatibility
of the generated public API alone, and compatibility of both the API and the build integration that
consumers use.

#### Scenario: Both compatibility levels are covered
- **WHEN** the report evaluates a separate tool
- **THEN** it states the savings available and the effort required at each compatibility level

### Requirement: Recorded decision
The analysis SHALL end in a decision recorded as an ADR. The decision SHALL be exactly one of:
- go upstream;
- go separate tool;
- a mix of the two;
- no-go.

It SHALL state the reasons, trace to the report's findings, and be made by the project owner after the
numbers are reviewed.

#### Scenario: Decision traces to findings
- **WHEN** the decision ADR is read
- **THEN** it names one of the four outcomes and cites the findings and numbers that support it

#### Scenario: Upstream PR counts as a go
- **WHEN** the worthwhile findings can all be delivered as upstream changes
- **THEN** the outcome is "go upstream", not "no-go"
