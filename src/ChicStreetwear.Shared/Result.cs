using ChicStreetwear.Shared.Interfaces;

namespace ChicStreetwear.Shared;
public class Result<T> : IResult
{
    private readonly T? _data = default;

    private readonly List<Error>? _errors = null;

    public bool HasErrors { get; }
    public T Data => _data!;
    public List<Error> Errors => HasErrors ? _errors! : new();

    protected Result(T? data, List<Error>? errors, bool hasErrors)
    {
        _data = data;
        _errors = errors;
        HasErrors = hasErrors;
    }

    private Result(List<Error> errors)
    {
        _errors = errors;
        HasErrors = true;
    }

    private Result(T data)
    {
        _data = data;
        HasErrors = false;
    }

    public static Result<T> Succeeded(T data)
    {
        return new(data);
    }

    public static Result<T> Failed(Error error)
    {
        return new(new List<Error>() { error });
    }

    public static Result<T> Failed(List<Error> errors)
    {
        return new(errors);
    }

    public static Result<T> ValidationFailed(List<Error> errors)
    {
        return new(errors);
    }

    public static implicit operator Result<T>(List<Error> errors) => new(errors);
    public static implicit operator Result<T>(Error error) => new(new List<Error>() { error });
}
