namespace ChicStreetwear.Shared.Interfaces;
public interface IError
{
    string Key { get; }
    string Message { get; }
    int StatusCode { get; }
}
