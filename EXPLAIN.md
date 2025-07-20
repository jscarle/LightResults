# LightResults API Overview

LightResults implements a lightweight Result Pattern for .NET. Use it to represent success or failure states without exceptions.

## Core Types

### `Result`
Represents a success or failure without a value.

Static factory methods that return a `Result`:
- `Result.Success()` – create a success.
- `Result.Failure()` – create a failure with an empty error.
- `Result.Failure(string message)` – failure with an error message.
- `Result.Failure(string message, (string Key, object? Value) metadata)`
- `Result.Failure(string message, KeyValuePair<string, object?> metadata)`
- `Result.Failure(string message, IReadOnlyDictionary<string, object?> metadata)`
- `Result.Failure(Exception? ex)` – failure wrapping an exception.
- `Result.Failure(string message, Exception? ex)` – message and exception.
- `Result.Failure(IError error)` – failure from an existing error.
- `Result.Failure(IEnumerable<IError> errors)`
- `Result.Failure(IReadOnlyList<IError> errors)`

Static factory methods that return a `Result<TValue>`:
- `Result.Success<TValue>()` – create a success.
- `Result.Failure<TValue>()` – create a failure with an empty error.
- `Result.Failure<TValue>(string message)` – failure with an error message.
- `Result.Failure<TValue>(string message, (string Key, object? Value) metadata)`
- `Result.Failure<TValue>(string message, KeyValuePair<string, object?> metadata)`
- `Result.Failure<TValue>(string message, IReadOnlyDictionary<string, object?> metadata)`
- `Result.Failure<TValue>(Exception? ex)` – failure wrapping an exception.
- `Result.Failure<TValue>(string message, Exception? ex)` – message and exception.
- `Result.Failure<TValue>(IError error)` – failure from an existing error.
- `Result.Failure<TValue>(IEnumerable<IError> errors)`
- `Result.Failure<TValue>(IReadOnlyList<IError> errors)`

Instance members:
- `bool IsSuccess()`
- `bool IsFailure()`
- `bool IsFailure(out IError error)` – returns first error.
- `bool HasError<TError>()` / `bool HasError<TError>(out TError error)` – check for a specific error type.
- `Result AsFailure()` – convert to failure `Result`.
- `Result<TDestination> AsFailure<TDestination>()` – convert to failure of another type.
- `IReadOnlyCollection<IError> Errors` – full error list.

### `Result<T>`
Generic result carrying a value on success. Additional members:
- `bool IsSuccess(out TValue value)` – get value when successful.
- `bool IsSuccess(out TValue value, out IError error)` – also obtains first error when failed.
- `bool IsFailure(out IError error, out TValue value)` – obtains value on failure (default) and error.
- `bool HasError<TError>()` / `bool HasError<TError>(out TError error)` – check for a specific error type.
- `Result AsFailure()` – convert to failure `Result`.
- `Result<TDestination> AsFailure<TDestination>()` – convert to failure of another type.
- `IReadOnlyCollection<IError> Errors` – full error list.
- Implicit conversions: from `TValue` to success result and from `Error` to failure result.

### `Error`
Represents an error with optional metadata.

Constructors:
- `Error()` – empty error.
- `Error(string message)`
- `Error(Exception? ex)` – message taken from exception.
- `Error(string message, Exception? ex)`
- `Error(string message, (string Key, object? Value) metadata)`
- `Error(string message, KeyValuePair<string, object?> metadata)`
- `Error(string message, IEnumerable<KeyValuePair<string, object?>> metadata)` (only on .NET 6 or higher)
- `Error(string message, IReadOnlyDictionary<string, object?> metadata)`

Properties – available on both `Error` and `IError`:
- `string Message`
- `IReadOnlyDictionary<string, object?> Metadata`
- `Exception? Exception` (if present in metadata)
- `static IError Empty`

Implements `IEquatable<Error>` with `==` and `!=` operators.

### Interfaces
- `IError` – exposes `Message`, `Metadata`, and `Exception`.
- `IResult` – exposes `Errors`, `IsSuccess`, `IsFailure`, and `HasError` methods.
- `IResult<T>` – adds overloads of `IsSuccess` and `IsFailure` to access the value.
- `IActionableResult` / `IActionableResult<TValue, TResult>` (for .NET 7+) – interfaces with static abstract members so generic code can create results via `TResult.Success(...)`/`TResult.Failure(...)`.

## Usage Pattern
1. Create results using the static `Result` methods rather than constructors.
2. Check results with `IsSuccess()`/`IsFailure()` before accessing values or errors.
3. Use `HasError<TError>()` to branch on specific error types.
4. Custom errors can inherit from `Error` to represent domain-specific failures.
5. Convert failed results to another result type with `AsFailure()` or `AsFailure<T>()`.
6. Prefer returning `Result` or `Result<T>` from methods rather than throwing exceptions.

## Target Frameworks
The library targets .NET Standard 2.0 and .NET 6.0–9.0. It's AOT compatible and optimised for minimal allocations.

