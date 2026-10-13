# Spec Delta

## Purpose

Provides a reproducible, version-pinned set of Protobuf message and service shapes, the C# code the
standard toolchain generates for them, and proof that the code behaves correctly, as the subject of
the allocation analysis.

## ADDED Requirements

### Requirement: Shape coverage
The corpus SHALL contain at least one definition for each shape of interest:
- scalar fields, string fields and bytes fields;
- packed and unpacked repeated fields, and map fields;
- nested messages at several depths;
- oneofs, including scalar members;
- well-known wrapper types and proto3 `optional` fields;
- unary, server-streaming, client-streaming and bidirectional-streaming services.

#### Scenario: Every shape is represented
- **WHEN** the corpus is compared with the shape list
- **THEN** each listed shape maps to at least one named definition, and the mapping is documented

#### Scenario: Unknown fields are exercised
- **WHEN** a payload containing fields unknown to the receiving definition is parsed
- **THEN** the corpus includes a definition and payload for that case

### Requirement: Pinned subject versions
The generated code SHALL be produced by exact, recorded versions of the code generator package and the
Protobuf runtime. Changing either version SHALL be an explicit, reviewable change.

#### Scenario: Versions are exact
- **WHEN** the recorded generator and runtime versions are inspected
- **THEN** each is a single exact version, not a range or floating version

### Requirement: Committed generated code
The C# generated from the corpus SHALL be stored in the repository so that findings can cite it by file
and line. It SHALL match what the pinned generator produces from the current corpus.

#### Scenario: Generated code is current
- **WHEN** the build regenerates code from the corpus with the pinned generator
- **THEN** the regenerated files are identical to the committed files

#### Scenario: Version bump shows as a diff
- **WHEN** a pinned generator or runtime version is changed and the code regenerated
- **THEN** every change to generated code appears in the repository diff

### Requirement: Round-trip correctness
Every corpus message SHALL serialize and parse back to a value equal to the original, for
representative populated, empty and edge-case values.

#### Scenario: Populated message round-trips
- **WHEN** a fully populated instance of any corpus message is serialized and then parsed
- **THEN** the parsed instance equals the original

#### Scenario: Unknown fields survive a round trip
- **WHEN** a payload with unknown fields is parsed and re-serialized
- **THEN** the unknown fields are preserved in the output bytes

#### Scenario: Broken round trip fails the test gate
- **WHEN** any corpus message fails to round-trip
- **THEN** the test gate exits non-zero
