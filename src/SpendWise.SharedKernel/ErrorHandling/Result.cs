using System.Runtime.InteropServices;

namespace SpendWise.SharedKernel.ErrorHandling;

public class Result
{
    public Error Error { get; }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("A successful result cannot contain an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("A failure result must contain an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true, Error.None);

    public static Result Failure(Error error)
        => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error)
        => new(default, false, error);

    public static Result<TValue> Create<TValue>(TValue? value)
        => value is not null
            ? Success(value)
            : Failure<TValue>(Error.NullValue);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;
    protected internal Result(TValue? value, bool isSuccess, Error error) 
        : base(isSuccess, error) => _value = value;
    
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static implicit operator Result<TValue>(TValue? value)
        => Create(value);
}

