using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LightResults;

[StructLayout(LayoutKind.Auto)]
[AsyncMethodBuilder(typeof(ResultValueTaskMethodBuilder<>))]
public struct ResultValueTask<TResult>
{
    internal ValueTask<TResult> ValueTask;

    internal ResultValueTask(ValueTask<TResult> valueTask)
    {
        ValueTask = valueTask;
    }
    
    public ResultValueTaskAwaiter<TResult> GetAwaiter()
    {
        return new ResultValueTaskAwaiter<TResult>(this);
    }
}

public readonly record struct ResultValueTaskAwaiter<TResult> : ICriticalNotifyCompletion
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

    private readonly ValueTask<TResult> _valueTask;
    private readonly ValueTaskAwaiter<TResult> _valueTaskAwaiter;

    public ResultValueTaskAwaiter(ResultValueTask<TResult> resultValueTask)
    {
        _valueTask = resultValueTask.ValueTask;
        _valueTaskAwaiter = resultValueTask.ValueTask.GetAwaiter();
    }

    public TResult GetResult()
    {
        return _valueTask.Result;
    }
}

[StructLayout(LayoutKind.Auto)]
public record struct ResultValueTaskMethodBuilder<TResult>
{
    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.Task"/>
    public ResultValueTask<TResult> Task => new(_internalBuilder.Task);

    private AsyncValueTaskMethodBuilder<TResult> _internalBuilder;

    public ResultValueTaskMethodBuilder()
    {
        _internalBuilder = new AsyncValueTaskMethodBuilder<TResult>();
    }

    /// <inheritdoc cref="AsyncValueTaskMethodBuilder{TResult}.Create"/>
    public static ResultValueTaskMethodBuilder<TResult> Create()
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
    public void SetResult(TResult result)
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
