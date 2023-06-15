using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Application.Mappers;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ChicStreetwear.Application.Categories.Queries;

public sealed class GetCategoriesDataGridQuery : GetQueryBase, IRequest<Result<PaginatedListModel<CategoryDataGridItemModel>>> { }

public sealed class GetCategoriesDataGridQueryHandler : IRequestHandler<GetCategoriesDataGridQuery, Result<PaginatedListModel<CategoryDataGridItemModel>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetCategoriesDataGridQueryHandler> _logger;

    public GetCategoriesDataGridQueryHandler(ICategoryRepository categoryRepository, IProductRepository productRepository, ILogger<GetCategoriesDataGridQueryHandler> logger)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Result<PaginatedListModel<CategoryDataGridItemModel>>> Handle(GetCategoriesDataGridQuery request, CancellationToken cancellationToken)
    {
        var categoriesDataGrid = await _categoryRepository.ListAsync(request.Sorts, request.Search, request.Page, request.Take, cancellationToken);

        var categoriesIds = await _productRepository.GetCategoriesIdsFromProductCategoryAsync(cancellationToken);

        Dictionary<Guid, int> categoryCounter = new();
        foreach (var cId in categoriesIds)
        {
            if (categoryCounter.TryGetValue(cId, out var count))
            {
                categoryCounter[cId] = count + 1;
                continue;
            }
            categoryCounter[cId] = 1;
        }

        return Result<PaginatedListModel<CategoryDataGridItemModel>>.Succeeded(new(categoriesDataGrid.Items.ConvertAll(cdgi => cdgi.ToCategoryItemDataGridModel(categoryCounter.GetValueOrDefault(cdgi.Id))), categoriesDataGrid.TotalItems));
    }
}

