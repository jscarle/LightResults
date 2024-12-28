[![Banner](https://raw.githubusercontent.com/jscarle/LightResults/main/Banner.png)](https://github.com/jscarle/LightResults)

# LightResults - Operation Result Patterns for .NET

LightResults is an extremely light and modern .NET library that provides a simple and flexible
implementation of the Result Pattern. The Result Pattern is a way of representing the outcome
of an operation, whether it's successful or has encountered an error, in a more explicit and
structured manner. This project is heavily inspired by [Michael Altmann](https://github.com/altmann)'s
excellent work with [FluentResults](https://github.com/altmann/FluentResults).

[![main](https://img.shields.io/github/actions/workflow/status/jscarle/LightResults/main.yml?logo=github)](https://github.com/jscarle/LightResults)
[![nuget](https://img.shields.io/nuget/v/LightResults)](https://www.nuget.org/packages/LightResults)
[![downloads](https://img.shields.io/nuget/dt/LightResults)](https://www.nuget.org/packages/LightResults)

## References

This library targets .NET Standard 2.0, .NET 6.0, .NET 7.0, .NET 8.0, and .NET 9.0.

## Dependencies

This library has no dependencies.

## Advantages of this library

- 🪶 Lightweight — Only contains what's necessary to implement the Result Pattern.
- ⚙️ Extensible — Simple interfaces and base classes make it easy to adapt.
- 🧱 Immutable — Results and errors are immutable and cannot be changed after being created.
- 🧵 Thread-safe — The Error list and Metadata dictionary use Immutable classes for thread-safety.
- ✨ Modern — Built against the latest version of .NET using the most recent best practices.
- 🧪 Native — Written, compiled, and tested against the latest versions of .NET.
- ❤️ Compatible — Available for dozens of versions of .NET as a
  [.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) library.
- 🪚 Trimmable — Compatible with [ahead-of-time compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) (AOT) as of .NET 7.0.
- 🚀 Performant — Heavily optimized and [benchmarked](https://jscarle.github.io/LightResults/docs/performance.html) to aim for the highest possible performance.

## Extensions

Several [extensions are available](https://github.com/jscarle/LightResults.Extensions) to simplify implementation that use LightResults.

## Documentation

Make sure to [read the docs](https://jscarle.github.io/LightResults/) for the full API.

## Getting Started

LightResults consists of only three classes `Result`, `Result<TValue>`, and `Error`.

- The `Result` class represents a generic result indicating success or failure.
- The `Result<TValue>` class represents a success or failure result with a value.
- The `Error` class represents an error with a message and optional associated metadata.

### Creating a successful result

Successful results can be created using the `Success` method.

```csharp
var successResult = Result.Success();

var successResultWithValue = Result.Success(349.4);
```

### Creating a failure result

Failed results can be created using the `Failure` method.

```csharp
var failureResult = Result.Failure();

var failureResultWithMessage = Result.Failure("Operation failure!");

var failureResultWithMessageAndMetadata = Result.Failure("Operation failure!", ("UserId", userId));

var failureResultWithMessageAndException = Result.Failure("Operation failure!", ex);
```

### Checking the state of a result

There are two methods used to check a result, `IsSuccess()` and `IsFailed()`. Both of which have several overloads to obtain the
value and error.

```csharp
if (result.IsSuccess())
{
    // The result is successful.
}

if (result.IsFailure(out var error))
{
    // The result is failure.
    if (error.Message.Length > 0)
        Console.WriteLine(error.Message);
    else
        Console.WriteLine("An unknown error occured!");
}
```

### Getting the value

The value from a successful result can be retrieved through the `out` parameter of the `Success()` method.

```csharp
if (result.IsSuccess(out var value))
{
    Console.WriteLine($"Value is {value}");
}
```

### Creating errors

Errors can be created with or without a message.

```csharp
var errorWithoutMessage = new Error();

var errorWithMessage = new Error("Something went wrong!");
```

Or with a message and metadata.

```csharp
var errorWithMetadataTuple = new Error("Something went wrong!", ("Key", "Value"));

var metadata = new Dictionary<string, object> { { "Key", "Value" } };
var errorWithMetadataDictionary = new Error("Something went wrong!", metadata);
```

### Custom errors

The best way to represent specific errors is to make custom error classes that inherit from `Error`
and define the error message as a base constructor parameter.

```csharp
public sealed class NotFoundError : Error
{
    public NotFoundError()
        : base("The resource cannot be found.")
    {
    }
}

var notFoundError = new NotFoundError();
var notFoundResult = Result.Failure(notFoundError);
```

Then the result can be checked against that error type.

```csharp
if (result.IsFailure(out var error) && error is NotFoundError)
{
    // Looks like the resource was not found, we better do something about that!
}
```

Or checked to see if there are any errors of that type.

```csharp
if (result.IsFailure() && result.HasError<NotFoundError>())
{
    // At least one of the errors was a NotFoundError.
}
```

This can be especially useful when combined with metadata that is related to a specific type of error.

```csharp
public sealed class HttpError : Error
{
    public HttpError(HttpStatusCode statusCode)
        : base("An HTTP error occured.", ("StatusCode", statusCode))
    {
    }
}
```

We can further simplify creating errors by creating an error factory.

```csharp
public static AppError
{
    public Result NotFound()
    {
        var notFoundError = new NotFoundError();
        return Result.Failure(notFoundError);
    }

    public Result HttpError(HttpStatusCode statusCode)
    {
        var httpError = new HttpError(statusCode)
        return Result.Failure(httpError);
    }
}
```

Which clearly and explicitly describes the results.

```csharp
public Result GetPerson(int id)
{
    var person = _database.GetPerson(id);
    
    if (person is null)
        return AppError.NotFound();
    
    return Result.Success();
}
```

### Handling Exceptions

Specific overloads have been added to `Failure()` and `Failure<TValue>()` to simplify using try-catch blocks and return from them with a result instead of
throwing.

```csharp
public Result DoSomeWork()
{
    try
    {
        // We must not throw an exception in this method!
    }
    catch(Exception ex)
    {
        return Result.Failure(ex);
    }
    
    return Result.Success();
}
```

### Static abstract members in interfaces

_Note: Applies to .NET 7.0 (C# 11.0) or higher only!_

Thanks to the
[static abstract members in interfaces](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/static-abstract-interface-methods)
introduced in .NET 7.0 (C# 11.0), it is possible to use generics to obtain access to the methods
of the generic variant of the result. As such the error factory can be enhanced to take advantage of that.

```csharp
public static AppError
{
    public Result NotFound()
    {
        var notFoundError = new NotFoundError();
        return Result.Failure(notFoundError);
    }
    
    public TResult NotFound<TResult>()
    {
        var notFoundError = new NotFoundError(); 
        return TResult.Failure(notFoundError);
    }
}
```

## What's new in v9.0

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
