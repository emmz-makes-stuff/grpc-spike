# Spec Delta

## Purpose

Measures, repeatably, how much the standard generated message and service code allocates and how long
it takes, and attributes those allocations to the code that makes them, so the analysis rests on numbers.

## ADDED Requirements

### Requirement: Per-shape message operations are measured
For every corpus message shape, the suite SHALL measure parse, serialize, size calculation, merge and
clone. It SHALL report allocated bytes per operation, garbage-collection counts, and mean time.

#### Scenario: Each shape has each operation
- **WHEN** the benchmark suite is run in full
- **THEN** the results contain allocated bytes, GC counts and mean time for every shape and operation pair

### Requirement: Stub layer is measured in proportion
The suite SHALL measure the generated service stub layer at the serializer/deserializer level. It SHALL
also measure complete calls made in-process for unary and each streaming kind, so the stub layer's cost
can be compared with the whole call's cost.

#### Scenario: Marshaller and whole-call costs are both reported
- **WHEN** the stub benchmarks are run
- **THEN** results report allocations for the marshalling step alone and for the complete in-process call, for the same message

### Requirement: Allocation attribution
For any benchmark, the suite SHALL be able to produce a trace that attributes allocations to the types
allocated and to the call sites that allocate them.

#### Scenario: An allocation is traced to its source
- **WHEN** an attributed run is made for a chosen benchmark
- **THEN** the output names the allocated types and the methods allocating them, with their share of allocated bytes

### Requirement: Runtime comparison
The suite SHALL run on .NET 10 as the primary runtime and on .NET 11 as a comparison. Every result SHALL
be labelled with its runtime.

#### Scenario: Both runtimes appear in results
- **WHEN** the full suite is run
- **THEN** each benchmark has a .NET 10 result and a .NET 11 result, each labelled

### Requirement: Reference implementation row
The suite SHALL include an alternative, non-drop-in serializer as a reference row for comparable message
shapes. The reference row SHALL be marked as excluded from the go/no-go criteria.

#### Scenario: Reference row is labelled
- **WHEN** results are exported
- **THEN** the reference rows are distinguishable from the subject rows and are marked reference-only

### Requirement: Reproducible, exported results
Benchmark results SHALL be exported in human-readable and machine-readable forms and committed with the
repository. Each export SHALL record the environment:
- machine and OS;
- runtime;
- pinned subject versions.

Timing results SHALL be labelled as specific to that machine.

#### Scenario: Environment is recorded
- **WHEN** an exported result file is inspected
- **THEN** it states the hardware, OS, runtime and subject versions it was measured with

#### Scenario: Benchmarks are not a gate
- **WHEN** the build, test and format gates run
- **THEN** the benchmark suite is not executed by them, and runs only when invoked explicitly
