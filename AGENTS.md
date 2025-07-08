# Guidance for AI contributors

This repository contains **LightResults**, a very small but highly-optimized .NET library that implements the Result Pattern.  The code is intended to be used in performance critical paths of applications that handle millions of requests per second.  The current version targets multiple frameworks (netstandard2.0 and net6.0–net9.0) and relies heavily on low allocation techniques.

## Repository layout

- `src/LightResults` – Main library.
  - `Result.cs` / `Result\`1.cs` – immutable `readonly struct` implementations.
  - `Error.cs` – immutable error type.
  - `IResult.cs`, `IResult\`1.cs`, `IError.cs` – API contracts.
  - `Common/` – helper utilities and optional interfaces when targeting newer frameworks.
- `tests/LightResults.Tests` – xUnit tests for the library.
- `docfx/` – documentation generation config.
- `LightResults.sln` – solution file.

## Coding style

- C# files use 4 space indentation and braces on the next line.
- Private fields are named with leading underscores (`_errors`, `_isSuccess`).
- Public APIs use PascalCase.  Generic type parameters use the `TValue` naming convention.
- Avoid LINQ in hot paths – explicit `for` loops are used instead to reduce allocations.
- The library prefers `readonly struct` and `init` properties to keep allocations to a minimum and maintain immutability.
- String formatting is performance oriented (`string.Create` with pre-computed lengths) – follow existing patterns in `StringHelper.cs`.

## Performance considerations

- Do not introduce allocations inside frequently executed methods.  Use `readonly` collections and structs when possible.
- Exceptions should not be part of normal control flow.  Factory methods like `Result.Failure(Exception ex)` wrap exceptions into metadata instead of throwing.
- When iterating through errors, prefer `for` loops and avoid `foreach`/LINQ which allocate enumerators.
- The code is AOT compatible and compiled with optimizations enabled; keep new code trim-friendly.

## Building and testing

The project uses the standard .NET SDK.  When a .NET environment is available you can run:

```bash
# Restore and build
 dotnet build LightResults.sln

# Run tests
 dotnet test tests/LightResults.Tests/LightResults.Tests.csproj
```

However, building or testing is not required for simple documentation updates.  This repository currently does not include an automated setup script for installing the .NET SDK.

Do not restore, run, or build performance benchmarks found in `tools`.

## Adding features or fixes

1. Maintain API surface stability—public members are part of a widely used library.
2. Keep the implementation allocation free when possible and favour explicit loops and array usage.
3. Include or update unit tests under `tests/LightResults.Tests` when modifying behaviour.
4. The general documentation (`docfx` site) should not be updated manually, it is built using a GitHub Action `.github/workflows/docs.yml`.
5. Always make sure the README.md is up to date and matches the public API.

## Useful references

- `src/LightResults/Result.cs` and `src/LightResults/Result\`1.cs` show the patterns for allocating errors, success values and formatting output.
- `tests/LightResults.Tests` demonstrates expected behaviour of the API and how custom error types can be implemented.

