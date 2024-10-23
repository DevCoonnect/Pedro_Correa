namespace AuthMuseum.Core.Common;
public class Result
{
    public static readonly Result Success = new ();
    
    public virtual bool IsSuccess { get; }

    public virtual Error? Error { get; }

    public Result()
    {
        IsSuccess = true;
        Error = null;
    }

    public Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        IsSuccess = false;
        Error = error;
    }

    public virtual R Match<R>(Func<R> onSuccess, Func<Error, R> onFailed)
    {
        return IsSuccess
            ? onSuccess()
            : onFailed(Error!);
    }
    
    public virtual Result IfFail(Action<Error> func)
    {
        if (!IsSuccess)
            func(Error!);
        
        return this;
    }

    public virtual Result IfSuccess(Action func)
    {
        if (IsSuccess)
            func();

        return this;
    }

    public static implicit operator Result(Error error) => new(error); // This makes a implicit conversion from Error to Result. So all params that accepts Result, an Error can be passed.
}

public class Result<T> : Result
{
    public T? Value { get; }

    public Result(T value)
    {
        Value = value;
    }

    public Result(Error error) : base(error)
    {
        Value = default;
    }

    public R Match<R>(Func<T, R> onSuccess, Func<Error, R> onFailed)
    {
        return IsSuccess
            ? onSuccess(Value!)
            : onFailed(Error!);
    }
    
    public Result<T> IfSuccess(Action<T> func)
    {
        if (IsSuccess)
            func(Value!);

        return this;
    }
    

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Error error) => new(error);
}
