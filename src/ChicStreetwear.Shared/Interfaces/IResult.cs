namespace ChicStreetwear.Shared.Interfaces;
public interface IResult
{
    bool HasErrors { get; }
    List<Error>? Errors { get; }
}
