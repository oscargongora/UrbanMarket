
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class PictureErrors
{
    public static Error NotFound => Error.NotFound("Picture.NotFound", "No picture found.");
}
