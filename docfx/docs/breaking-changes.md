# What's new in v9.0

The API for LightResults was completely redesigned for v9.0 to improve performance and remove any potential pits of failure caused by the prior version's use
of properties that could result in exceptions being thrown when values were accessed without checking the state of the result. Thus, there are several breaking
changes, detailed below, that developers must be aware of when upgrading from v8.0 to v9.0.

### Breaking changes between v8.0 and v9.0

- Factory methods for creating generic results have changed names.
    - `Result.Ok()` has been renamed to `Result.Success()`.
    - `Result.Fail()` has been renamed to `Result.Failure()`.
- Factory methods for creating results with values have changed names and type to allow omitting the type when it is known.
    - `Result<TValue>.Ok()` has been renamed and moved to `Result.Success<TValue>()`.
    - `Result<TValue>.Fail()` has been renamed to `Result.Failure<TValue>()`.
- The `Value` and `Error` properties have been removed.
    - `result.Value` has been replaced by `result.IsSuccess(out var value)`.
    - `result.Error` has been replaced by `result.IsError(out var error)`.
- Several constructors of the `Error` type have been removed or have changed.
    - `Error((string Key, object Value) metadata)` has been removed.
    - `Error(IDictionary<string, object> metadata)` has been removed.
    - `Error(string message, IDictionary<string, object> metadata)` has been changed to
      `Error(string message, IReadOnlyDictionary<string, object> metadata)`.

### Migrating

The following steps in the following order will reduce the amount of manual work required to migrate and refactor code to use the new API.

1. Find and replace all instances of `Result.Ok(` with `Result.Success(`.
2. Find and replace all instances of `Result.Fail(` with `Result.Failure(`.
3. Regex find and replace all instances of `Result(<[^>]+>)\.Ok\(` with `Result.Success$1(`.
4. Regex find and replace all instances of `Result(<[^>]+>)\.Fail\(` with `Result.Failure$1(`.
5. Find and replace all instances of `.IsSuccess` with `IsSuccess(out var value)`.
6. Find and replace all instances of `.IsFailed` with `IsFailure(out var error)`.
7. Find instances of `result.Value` and refactor them to the use the `value` exposed by the `IsSuccess()` method.
8. Find instances of `result.Error` and refactor them to the use the `error` exposed by the `IsFailure()` method.

### New method overloads and property initializers

- New overloads have been added for `KeyValuePair<string, object>` metadata.
    - `Result.Failure(string errorMessage, KeyValuePair<string, object> metadata)` has been added.
    - `Result.Failure<TValue>(string errorMessage, KeyValuePair<string, object> metadata)` has been added.
- New overloads have been added to simplify handling exceptions.
    - `Result.Failure(Exception ex)` has been added.
    - `Result.Failure(string errorMessage, Exception ex)` has been added.
    - `Result.Failure<TValue>(Exception ex)` has been added.
    - `Result.Failure<TValue>(string errorMessage, Exception ex)` has been added.
- New overloads where added to access the value.
    - `result.IsSuccess(out TValue value)` has been added.
    - `result.IsFailure(out IError error, out TValue value)` has been added.
- New overloads where added to access the first error.
    - `result.IsFailure(out IError error)` has been added.
    - `result.IsSuccess(out TValue value, out IError error)` has been added.
    - `result.HasError<TError>(out IError error)` has been added.
- New property initializers where added to `Error`.
    - `Message { get; }` has changed to `Message { get; init; }`.
    - `Metadata { get; }` has changed to `Metadata { get; init; }`.
