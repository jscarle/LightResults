# Getting Started

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

Specific overloads have been added to `Failure()` and `Failure<TValue>()` to simplify using try-catch blocks and return from them with a result instead of throwing.

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
