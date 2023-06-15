using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Aggregates.Category;

public sealed class Category : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Category() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Category(Guid id, string name, string description) : base(id)
    {
        Name = name;
        Description = description;
    }

    public static Result<Category> New(string name, string description)
    {
        Category newCategory = new(Guid.NewGuid(), name, description);

        return Result<Category>.Succeeded(newCategory);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}