# Result.Fail method (1 of 12)

Creates a failed result.

```csharp
public static Result Fail()
```

## Return Value

A new instance of [`Result`](../Result.md) representing a failed result.

## See Also

* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail method (2 of 12)

Creates a failed result with the given errors.

```csharp
public static Result Fail(IEnumerable<IError> errors)
```

| parameter | description |
| --- | --- |
| errors | A collection of errors associated with the failure. |

## Return Value

A new instance of [`Result`](../Result.md) representing a failed result with the specified errors.

## See Also

* interface [IError](../IError.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail method (3 of 12)

Creates a failed result with the given error.

```csharp
public static Result Fail(IError error)
```

| parameter | description |
| --- | --- |
| error | The error associated with the failure. |

## Return Value

A new instance of [`Result`](../Result.md) representing a failed result with the specified error.

## See Also

* interface [IError](../IError.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail method (4 of 12)

Creates a failed result with the given error message.

```csharp
public static Result Fail(string errorMessage)
```

| parameter | description |
| --- | --- |
| errorMessage | The error message associated with the failure. |

## Return Value

A new instance of [`Result`](../Result.md) representing a failed result with the specified error message.

## See Also

* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail method (5 of 12)

Creates a failed result with the given error message and metadata.

```csharp
public static Result Fail(string errorMessage, (string Key, object Value) metadata)
```

| parameter | description |
| --- | --- |
| metadata | The metadata associated with the failure. |
| errorMessage | The error message associated with the failure. |

## Return Value

A new instance of [`Result`](../Result.md) representing a failed result with the specified error message and metadata.

## See Also

* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail method (6 of 12)

Creates a failed result with the given error message and metadata.

```csharp
public static Result Fail(string errorMessage, IDictionary<string, object> metadata)
```

| parameter | description |
| --- | --- |
| metadata | The metadata associated with the failure. |
| errorMessage | The error message associated with the failure. |

## Return Value

A new instance of [`Result`](../Result.md) representing a failed result with the specified error message and metadata.

## See Also

* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail&lt;TValue&gt; method (7 of 12)

Creates a failed result.

```csharp
public static Result<TValue> Fail<TValue>()
```

| parameter | description |
| --- | --- |
| TValue | The type of the value in the result. |

## Return Value

A new instance of [`Result`](../Result-1.md) representing a failed result.

## See Also

* class [Result&lt;TValue&gt;](../Result-1.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail&lt;TValue&gt; method (8 of 12)

Creates a failed result with the given errors.

```csharp
public static Result<TValue> Fail<TValue>(IEnumerable<IError> errors)
```

| parameter | description |
| --- | --- |
| TValue | The type of the value in the result. |
| errors | A collection of errors associated with the failure. |

## Return Value

A new instance of [`Result`](../Result-1.md) representing a failed result with the specified errors.

## See Also

* class [Result&lt;TValue&gt;](../Result-1.md)
* interface [IError](../IError.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail&lt;TValue&gt; method (9 of 12)

Creates a failed result with the given error.

```csharp
public static Result<TValue> Fail<TValue>(IError error)
```

| parameter | description |
| --- | --- |
| TValue | The type of the value in the result. |
| error | The error associated with the failure. |

## Return Value

A new instance of [`Result`](../Result-1.md) representing a failed result with the specified error.

## See Also

* class [Result&lt;TValue&gt;](../Result-1.md)
* interface [IError](../IError.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail&lt;TValue&gt; method (10 of 12)

Creates a failed result with the given error message.

```csharp
public static Result<TValue> Fail<TValue>(string errorMessage)
```

| parameter | description |
| --- | --- |
| TValue | The type of the value in the result. |
| errorMessage | The error message associated with the failure. |

## Return Value

A new instance of [`Result`](../Result-1.md) representing a failed result with the specified error message.

## See Also

* class [Result&lt;TValue&gt;](../Result-1.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail&lt;TValue&gt; method (11 of 12)

Creates a failed result with the given error message and metadata.

```csharp
public static Result<TValue> Fail<TValue>(string errorMessage, (string Key, object Value) metadata)
```

| parameter | description |
| --- | --- |
| TValue | The type of the value in the result. |
| errorMessage | The error message associated with the failure. |
| metadata | The metadata associated with the failure. |

## Return Value

A new instance of [`Result`](../Result-1.md) representing a failed result with the specified error message and metadata.

## See Also

* class [Result&lt;TValue&gt;](../Result-1.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

---

# Result.Fail&lt;TValue&gt; method (12 of 12)

Creates a failed result with the given error message and metadata.

```csharp
public static Result<TValue> Fail<TValue>(string errorMessage, IDictionary<string, object> metadata)
```

| parameter | description |
| --- | --- |
| TValue | The type of the value in the result. |
| errorMessage | The error message associated with the failure. |
| metadata | The metadata associated with the failure. |

## Return Value

A new instance of [`Result`](../Result-1.md) representing a failed result with the specified error message and metadata.

## See Also

* class [Result&lt;TValue&gt;](../Result-1.md)
* class [Result](../Result.md)
* namespace [LightResults](../../LightResults.md)

<!-- DO NOT EDIT: generated by xmldocmd for LightResults.dll -->
