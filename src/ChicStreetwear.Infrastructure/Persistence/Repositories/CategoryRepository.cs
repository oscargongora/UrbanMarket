using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Infrastructure.Identity;
using ChicStreetwear.Shared.Models.Components;
using Microsoft.EntityFrameworkCore;

namespace ChicStreetwear.Infrastructure.Persistence.Repositories;
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ChicStreetwearIdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<PaginatedListModel<Category>> ListAsync(SortedDictionary<int, QuerySortProperty> sorts, string? search = null, int? page = null, int? take = null, CancellationToken cancellationToken = default)
    {
        search ??= string.Empty;

        string itemsQuery = $"""
            select category.Id, category.Name, category.Description, COUNT(productCategory.Id) as Products
            from Categories category
            left join ProductCategory productCategory 
            on category.Id=productCategory.CategoryId
            where category.Name like '%{search}%' or category.Description like '%{search}%'
            group by category.Id, category.Name, category.Description
            """;

        bool firstSort = true;
        foreach (var sort in sorts)
        {
            if (firstSort)
            {
                itemsQuery += $"\nORDER BY {sort.Value.Name} {sort.Value.Order}";
                firstSort = false;
                continue;
            }
            itemsQuery += $",{sort.Value.Name} {sort.Value.Order}";
        }
        if (firstSort)
        {
            itemsQuery += $"\nORDER BY Name asc";
        }

        if (page is not null && take is not null)
        {
            itemsQuery += $"\nOFFSET {(page - 1) * take} ROWS FETCH NEXT {take} ROWS ONLY";
        }

        var items = await _dbContext.Categories
            .FromSqlRaw(itemsQuery)
            .AsNoTracking()
            .ToListAsync();

        var totalItems = await _dbContext.Categories.CountAsync(category =>
            EF.Functions.Like(category.Name, $"%{search}%") ||
            EF.Functions.Like(category.Description, $"%{search}%"));

        return new PaginatedListModel<Category>(items, totalItems);
    }
}
