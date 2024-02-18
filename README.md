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

This library targets .NET Standard 2.0, .NET 6.0, .NET 7.0, and .NET 8.0.

## Dependencies

This library has no dependencies.

## Advantages of this library

- 🪶 Lightweight — Only contains what's necessary to implement the Result Pattern.
- ⚙️ Extensible — Simple interfaces and base classes make it easy to adapt.
- 🧱 Immutable — Results and errors are immutable and cannot be changed after being created.
- 🧵 Thread-safe — The Error list and Metadata dictionary use Immutable classes for thread-safety.
- ✨ Modern — Built against the latest version of .NET using the most recent best practices.
- 🧪 Native — Written, compiled, and tested against the latest versions of .NET.
- ❤️ Compatible — Available for dozens of versions of .NET as a [.NET Standard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0) library.
- 🪚 Trimmable — Compatible with [ahead-of-time compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/) (AOT) as of .NET 7.0.
- 🚀 Performant — Heavily optimized and [benchmarked](https://jscarle.github.io/LightResults/docs/performance.html) to aim for the highest possible performance.

## Documentation

Make sure to [read the docs](https://jscarle.github.io/LightResults/) for the full API.

## Getting Started

LightResults consists of only three classes `Result`, `Result<TValue>`, and `Error`.

- The `Result` class represents a generic result indicating success or failure.
- The `Result<TValue>` class represents a result with a value.
- The `Error` class represents an error with a message and associated metadata.

### Creating a successful result

Successful results can be created using the `Ok` method.

```csharp
var successResult = Result.Ok();

var successResultWithValue = Result.Ok(349.4);
```

### Creating a failed result

Failed results can be created using the `Fail` method.

```csharp
var failedResult = Result.Fail();

var failedResultWithMessage = Result.Fail("Operation failed!");

var failedResultWithMessageAndMetadata = Result.Fail("Operation failed!", ("Exception", ex));
```

### Checking the state of a result

There are two properties for results, `IsSuccess` and `IsFailed`. Both are mutually exclusive.

```csharp
if (result.IsSuccess)
{
    // The result is successful therefore IsFailed will be false.
}

if (result.IsFailed)
{
    // The result is failed therefore IsSuccess will be false.
    var firstError = result.Errors.First();
    if (firstError.Message.Length > 0)
        Console.WriteLine(firstError.Message);
    else
        Console.WriteLine("An unknown error occured!");
}
```

### Getting the value

The value from a successful result can be retrieved through the `Value` property.

```csharp
if (result.IsSuccess)
{
    var value = result.Value;
}
```

### Creating errors

Errors can be created with or without a message.

```csharp
var errorWithoutMessage = new Error();

var errorWithMessage = new Error("Something went wrong!");
```

With metadata.

```csharp
var errorWithMetadataTuple = new Error(("Key", "Value"));

var metadata = new Dictionary<string, object> { { "Key", "Value" } };
var errorWithMetadataDictionary = new Error(metadata);
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
var notFoundResult = Result.Fail(notFoundError);
```

Then the result can be checked against that error type.

```csharp
if (result.IsFailed && result.HasError<NotFound>())
{
    // Looks like the resource was not found, we better do something about that!
}
```

This can be especially useful when combined with metadata to handle exceptions.

```csharp
public sealed class UnhandledExceptionError : Error
{
    public UnhandledExceptionError(Exception ex)
        : base("An unhandled exception occured.", ("Exception", ex))
    {
    }
}
```

Which allows us to continue using try-catch blocks in our code but return from them with a result instead of throwing an exception.

```csharp
public Result DoSomeWork()
{
    try
    {
        // We must not throw an exception in this method!
    }
    catch(Exception ex)
    {
        var unhandledExceptionError = new UnhandledExceptionError(ex);
        return Result.Fail(unhandledExceptionError);
    }
    
    return Result.Ok();
}
```

We can further simplify creating errors by creating an error factory.

```csharp
public static AppError
{
    public Result NotFound()
    {
        var notFoundError = new NotFoundError();
        return Result.Fail(notFoundError);
    }

    public Result UnhandledException(Exception ex)
    {
        var unhandledExceptionError = new UnhandledExceptionError(ex)
        return Result.Fail(unhandledExceptionError);
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
    
    return Result.Ok();
}
```

## Static abstract members in interfaces

_Note: Applies to .NET 7.0 (C# 11.0) or higher only!_

Thanks to the [static abstract members in interfaces](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/6.0/static-abstract-interface-methods)
introduced in .NET 7.0 (C# 11.0), it is possible to use generics to obtain access to the methods
of the generic variant of the result. As such the error factory can be enhanced to take advantage of that.

```csharp
public static AppError
{
    public Result NotFound()
    {
        var notFoundError = new NotFoundError();
        return Result.Failt(notFoundError);
    }
    
    public TResult NotFound<TResult>()
    {
        var notFoundError = new NotFoundError(); 
        return TResult.Fail(notFoundError);
    }
}
```

