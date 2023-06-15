using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class RatingErrors
{
    public static Error InvalidValue => Error.New("Rating.InvalidValue", "Rating values must be between 1 and 5.", Shared.Enums.ErrorType.Validation);

}
