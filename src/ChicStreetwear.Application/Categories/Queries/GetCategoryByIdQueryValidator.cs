using FluentValidation;

namespace ChicStreetwear.Application.Categories.Queries;
public sealed class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(category => category.Id).NotNull();
    }
}
