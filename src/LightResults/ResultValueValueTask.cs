using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LightResults;

[StructLayout(LayoutKind.Auto)]
[AsyncMethodBuilder(typeof(ResultValueValueTaskMethodBuilder<>))]
public struct ResultValueValueTask<TValue>
{
    internal ValueTask<TValue> ValueTask;

    internal ResultValueValueTask(ValueTask<TValue> valueTask)
    {
        ValueTask = valueTask;
    }
    
    public ResultValueValueTaskAwaiter<TValue> GetAwaiter()
    {
        return new ResultValueValueTaskAwaiter<TValue>(this);
    }
}

public readonly record struct ResultValueValueTaskAwaiter<TValue> : ICriticalNotifyCompletion
{
    public bool IsCompleted => _valueTask.IsCompleted;

    public void OnCompleted(Action continuation)
    {
        _valueTaskAwaiter.OnCompleted(continuation);
    }

    public void UnsafeOnCompleted(Action continuation)
    {
        _valueTaskAwaiter.UnsafeOnCompleted(continuation);
    }

    private readonly ValueTask<TValue> _valueTask;
    private readonly ValueTaskAwaiter<TValue> _valueTaskAwaiter;

    public ResultValueValueTaskAwaiter(ResultValueValueTask<TValue> resultValueTask)
    {
        _valueTask = resultValueTask.ValueTask;
        _valueTaskAwaiter = resultValueTask.ValueTask.GetAwaiter();
    }

    public Result<TValue> GetResult()
    {
        return _valueTask.Result;
    }
}

[StructLayout(LayoutKind.Auto)]
public record struct ResultValueValueTaskMethodBuilder<TValue>
{
    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.Task"/>
    public ResultValueValueTask<TValue> Task => new(_internalBuilder.Task);

    private AsyncValueTaskMethodBuilder<TValue> _internalBuilder;

    public ResultValueValueTaskMethodBuilder()
    {
        _internalBuilder = new AsyncValueTaskMethodBuilder<TValue>();
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.Create"/>
    public static ResultValueValueTaskMethodBuilder<TValue> Create()
    {
        return default;
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.Start{TStateMachine}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        _internalBuilder.Start(ref stateMachine);
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.SetStateMachine"/>
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
        _internalBuilder.SetStateMachine(stateMachine);
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.SetResult"/>
    public void SetResult(TValue result)
    {
        _internalBuilder.SetResult(result);
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.SetException"/>
    public void SetException(Exception exception)
    {
        _internalBuilder.SetException(exception);
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.AwaitOnCompleted{TAwaiter, TStateMachine}"/>
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _internalBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.AwaitUnsafeOnCompleted{TAwaiter, TStateMachine}"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        _internalBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
    }
}
