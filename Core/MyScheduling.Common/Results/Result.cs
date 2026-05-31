namespace MyScheduling.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException("Resultado de sucesso não pode conter erro.");

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException("Resultado de falha deve conter erro.");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(value, true, Error.None);
    public static Result<T> Failure<T>(Error error) => new(default!, false, error);
}

public sealed class Result<T> : Result
{
    private readonly T _value;

    public T Value => IsSuccess
        ? _value
        : throw new InvalidOperationException("Não é possível acessar Value em um resultado de falha.");

    internal Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }
}
