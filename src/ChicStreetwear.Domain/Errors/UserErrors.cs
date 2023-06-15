using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class UserErrors
{
    public static Error InvalidNameIdentifier => Error.BadRequest("User.InvalidNameIdentifier", "The name identifier is invalid.");

    public static Error NotFound => Error.BadRequest("User.NotFound", "User not found.");
}
