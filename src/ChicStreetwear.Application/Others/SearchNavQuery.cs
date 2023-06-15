using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Domain.Aggregates.Category;
using ChicStreetwear.Domain.Aggregates.Product;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Others;
using MediatR;

namespace ChicStreetwear.Application.Others;

public sealed class SearchNavQuery : IRequest<Result<IEnumerable<SearchNavQueryResponseModel>>>
{
    public required string SearchText { get; set; }
}

public sealed class SearchNavQueryHandle : IRequestHandler<SearchNavQuery, Result<IEnumerable<SearchNavQueryResponseModel>>>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public SearchNavQueryHandle(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<IEnumerable<SearchNavQueryResponseModel>>> Handle(SearchNavQuery request, CancellationToken cancellationToken)
    {
        List<SearchNavQueryResponseModel> searchResult = new();

        var products = await _productRepository.PaginatedListAsync(new() { p => p.Name.Contains(request.SearchText) }, new(), 1, 3, true, cancellationToken);

        searchResult.AddRange(products.Items.Select(p => NewFromProduct(p)));

        var categories = await _categoryRepository.PaginatedListAsync(new() { p => p.Name.Contains(request.SearchText) }, new(), 1, 3, true, cancellationToken);

        searchResult.AddRange(categories.Items.Select(p => NewFromCategory(p)));

        searchResult.OrderBy(sr => sr.Title);
        return Result<IEnumerable<SearchNavQueryResponseModel>>.Succeeded(searchResult);
    }

    private SearchNavQueryResponseModel NewFromCategory(Category category)
    {
        return new() { Id = category.Id, Title = category.Name, Type = category.GetType().Name, ThumbnailUrl = null };
    }

    private SearchNavQueryResponseModel NewFromProduct(Product product)
    {
        return new() { Id = product.Id, Title = product.Name, Type = product.GetType().Name, ThumbnailUrl = product?.CoverPicture?.ThumbnailUrl };
    }
}

