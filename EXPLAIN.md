# LightResults API Overview

LightResults implements a lightweight Result Pattern for .NET. It provides immutable types to represent success or failure states without using exceptions.

## Namespaces
- `LightResults` – contains `Result`, `Result<TValue>`, `Error`, `IResult`, `IResult<TValue>` and `IError`.
- `LightResults.Common` – defines `IActionableResult<TResult>` and `IActionableResult<TValue, TResult>` (only available when targeting .NET 7 or later).

## Core Types

### `Result` (readonly struct)
Represents a success or failure without a value.

Static factory methods returning `Result`:
- `Result.Success()` – create a success instance.
- `Result.Failure()` – failure with an empty error.
- `Result.Failure(string message)`
- `Result.Failure(string message, (string Key, object? Value) metadata)`
- `Result.Failure(string message, KeyValuePair<string, object?> metadata)`
- `Result.Failure(string message, IReadOnlyDictionary<string, object?> metadata)`
- `Result.Failure(Exception? ex)` – wraps an exception.
- `Result.Failure(string message, Exception? ex)`
- `Result.Failure(IError error)`
- `Result.Failure(IEnumerable<IError> errors)`
- `Result.Failure(IReadOnlyList<IError> errors)`

Static factory methods returning `Result<TValue>`:
- `Result.Success<TValue>(TValue value)`
- `Result.Failure<TValue>()`
- `Result.Failure<TValue>(string message)`
- `Result.Failure<TValue>(string message, (string Key, object? Value) metadata)`
- `Result.Failure<TValue>(string message, KeyValuePair<string, object?> metadata)`
- `Result.Failure<TValue>(string message, IReadOnlyDictionary<string, object?> metadata)`
- `Result.Failure<TValue>(Exception? ex)`
- `Result.Failure<TValue>(string message, Exception? ex)`
- `Result.Failure<TValue>(IError error)`
- `Result.Failure<TValue>(IEnumerable<IError> errors)`
- `Result.Failure<TValue>(IReadOnlyList<IError> errors)`

Instance members:
- `bool IsSuccess()`
- `bool IsFailure()`
- `bool IsFailure(out IError error)` – retrieves the first error.
- `bool HasError<TError>()` / `bool HasError<TError>(out TError error)`
- `Result AsFailure()` – convert to a non-generic failure.
- `Result<TDestination> AsFailure<TDestination>()` – convert to a different failure type.
- `IReadOnlyCollection<IError> Errors`
- Implicit conversion from `Error` to `Result`.
- Equality: implements `IEquatable<Result>`, provides `Equals(...)`, `GetHashCode()`, and `==`/`!=` operators. Overrides `ToString()`.

### `Result<TValue>` (readonly struct)
Represents a result carrying a value on success.

Instance members:
- `bool IsSuccess()`
- `bool IsSuccess(out TValue value)`
- `bool IsSuccess(out TValue value, out IError error)`
- `bool IsFailure()`
- `bool IsFailure(out IError error)`
- `bool IsFailure(out IError error, out TValue value)`
- `bool HasError<TError>()` / `bool HasError<TError>(out TError error)`
- `Result AsFailure()`
- `Result<TDestination> AsFailure<TDestination>()`
- `IReadOnlyCollection<IError> Errors`
- Implicit conversions: `TValue` → success result, `Error` → failure result.
- Equality: implements `IEquatable<Result<TValue>>`, provides `Equals(...)`, `GetHashCode()`, and `==`/`!=` operators. Overrides `ToString()`.

### `Error` (class)
Represents an error with an optional message and metadata.

Constructors:
- `Error()` – empty error.
- `Error(string message)`
- `Error(Exception? ex)` – message from the exception stored under the `"Exception"` key.
- `Error(string message, Exception? ex)` – stores the exception under the `"Exception"` key.
- `Error(string message, (string Key, object? Value) metadata)`
- `Error(string message, KeyValuePair<string, object?> metadata)`
- `Error(string message, IEnumerable<KeyValuePair<string, object?>> metadata)` (available on .NET 6 or later)
- `Error(string message, IReadOnlyDictionary<string, object?> metadata)`

Properties:
- `string Message`
- `IReadOnlyDictionary<string, object?> Metadata`
- `Exception? Exception`
- `static IError Empty`

Implements `IEquatable<Error>` with equality operators, `Equals(...)`, `GetHashCode()`, and overrides `ToString()`.

### Interfaces
- `IError`
- `IResult`
- `IResult<TValue>`
- `IActionableResult<TResult>` *(NET 7+)*
- `IActionableResult<TValue, TResult>` *(NET 7+)*

Both actionable-result interfaces expose static abstract members for `Success` and the various `Failure` overloads listed above.

## Usage Pattern
1. Create results using the static `Result` methods rather than constructors.
2. Check results with `IsSuccess()`/`IsFailure()` before accessing values or errors.
3. Use `HasError<TError>()` to branch on specific error types.
4. Custom errors can inherit from `Error` to represent domain-specific failures.
5. Convert failed results to another type with `AsFailure()` or `AsFailure<TValue>()`.
6. Prefer returning `Result` or `Result<TValue>` from methods instead of throwing exceptions.

## Target Frameworks
This library targets .NET Standard 2.0 and .NET 6.0–9.0 and is AOT-compatible.
