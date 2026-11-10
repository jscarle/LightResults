# Guidance for AI contributors

This repository contains **LightResults**, a very small but highly-optimized .NET library that implements the Result Pattern.
The code is intended for performance-critical paths that handle millions of requests per second. The current version targets
multiple frameworks (net8.0–net11.0) and relies heavily on low-allocation techniques.

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
- Public APIs use PascalCase. Generic type parameters use the `TValue` naming convention.
- Avoid LINQ in hot paths – explicit `for` loops are used instead to reduce allocations.
- The library prefers `readonly struct` and `init` properties to keep allocations to a minimum and maintain immutability.
- String formatting is performance oriented (`string.Create` with pre-computed lengths) – follow existing patterns in `StringHelper.cs`.

## Performance considerations

- Do not introduce allocations inside frequently executed methods. Use `readonly` collections and structs when possible.
- Exceptions should not be part of normal control flow. Factory methods like `Result.Failure(Exception ex)` wrap exceptions into metadata instead of throwing.
- When iterating through errors, prefer `for` loops and avoid `foreach`/LINQ which allocate enumerators.
- The code is AOT compatible and compiled with optimizations enabled; keep new code trim-friendly.

## Building and testing

The project uses the standard .NET SDK. When a .NET environment is available you can run:

```bash
# Restore and build
 dotnet build tests/LightResults.Tests/LightResults.Tests.csproj

# Run tests
 dotnet test tests/LightResults.Tests/LightResults.Tests.csproj
```

The test project targets multiple frameworks; install the .NET 8.0–11.0 runtimes to test all targets, or specify one supported
framework, such as `-f net11.0`, to test only that target. Use the .NET 11 SDK selected by `global.json` to build the repository.

However, building or testing is not required for simple documentation updates. This repository currently does not include an automated setup script for installing the .NET SDK.

Do not restore, run, or build performance benchmarks found in `tools`.

## Adding features or fixes

1. Maintain API surface stability—public members are part of a widely used library.
2. Keep the implementation allocation free when possible and favour explicit loops and array usage.
3. Include or update unit tests under `tests/LightResults.Tests` when modifying behaviour.
4. The public documentation site is built automatically by `.github/workflows/docs.yml` from the `docfx` directory.
5. Always make sure the `README.md` and `docfx` is up to date and matches the public API.

## Useful references

- `src/LightResults/Result.cs` and `src/LightResults/Result\`1.cs` show the patterns for allocating errors, success values and formatting output.
- `tests/LightResults.Tests` demonstrates expected behaviour of the API and how custom error types can be implemented.

## Release Intent, Versioning, and Workflow

Treat explicit release requests—such as “Mint a new release,” “Release this,” “Publish this as a new release,” “Create a new release,” or any clear equivalent—as authorization to carry out LightResults' full release workflow, including committing, pushing, tagging, and publishing as needed.

If the user has not clearly requested a release, or if the appropriate change category cannot be determined confidently, ask for clarification **before** committing, pushing, creating a tag, or publishing a release.

Only change LightResults' package, assembly, and file versions when the user has clearly requested that a release be published. Do not bump versions during ordinary implementation, refactoring, bug-fixing, performance work, dependency updates, testing, commits, pushes, or pull-request preparation unless those actions are part of an explicitly requested release.

Choose the next version using these repository-specific rules:

- Minor, backward-compatible changes increment the patch component. Example: `10.0.4` → `10.0.5`.
- Moderate or breaking changes increment the middle component and reset the patch. Example: `10.0.4` → `10.1.0`.
- Increment the major component and reset the other components only when the newest supported .NET version changes. Example: `10.0.4` → `11.0.0`.
- Breaking changes alone do not justify a major-version increment in this repository. If multiple categories apply, use the highest applicable category.

Update `Version` in `src/LightResults/LightResults.csproj` using three components and update `AssemblyVersion` and `FileVersion` using the matching four-component form. Use a three-component lightweight tag such as `v10.0.5`; historical tag types are mixed, but new release tags must be lightweight.

A clear release request authorizes this complete workflow:

1. Inspect the worktree, current branch, `origin`, `develop`, recent commits, pull requests, and releases before editing release metadata.
2. Determine the next version, update the three version properties in `src/LightResults/LightResults.csproj`, and verify that the tag and release do not already exist.
3. Validate the release-scoped changes using the repository's normal restore, Release build, test, pack, and relevant performance-regression checks without running benchmark projects unless explicitly requested.
4. Commit all release-scoped changes on the reusable `feature/work` branch using a concise past-tense sentence ending in a period, then push `feature/work`.
5. Open a ready pull request from `feature/work` to `develop` using recent title and body conventions, enable squash auto-merge, and monitor checks and review state.
6. Fix and push any issue that prevents the pull request from merging, then wait until it is merged.
7. Fast-forward local `develop` from `origin/develop` and reset local `feature/work` to that latest `develop` commit. The remote feature branch may be deleted automatically after merge.
8. Create the lightweight `vX.Y.Z` tag on the latest `develop` commit and push that tag to `origin`.
9. Publish a non-draft GitHub release named `vX.Y.Z - Description`, using concise release-note bullets consistent with recent LightResults releases.
10. Monitor the release-triggered `Publish` workflow until the LightResults NuGet package is published successfully, fixing in-scope failures when possible.

Do not require separate confirmation for each Git or GitHub step after an unambiguous release command. Report the pull request, merge commit, tag, GitHub release URL, and NuGet publishing result when complete.
