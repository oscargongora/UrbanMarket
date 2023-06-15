using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Errors;
public static class CategoryErrors
{
    public static Error NotFound => Error.NotFound("Category.NotFound", "No category found.");
}
