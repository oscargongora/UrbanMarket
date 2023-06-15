using ChicStreetwear.Application.Products.Queries;
using ChicStreetwear.Shared.Models.Product;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ChicStreetwear.Server.Pages.Store;

record CarouselItem(string ImgSrc, string ImgSrcSet, string Heading, string SubHeading, string NavLinkText, string NavLinkHref);

record ServiceItem(string Icon, string Title, string Description);

record PickItem(string ImgSrc, string ImgSrcSet, string NavLinkText, string NavLinkHref);

public partial class Index
{
    [Inject]
    private IJSRuntime _jSRuntime { get; set; } = default!;

    [Inject]
    private ISender Sender { get; set; } = default!;

    List<CarouselItem> _carouselItems = new()
    {
        new("assets/carousel/carousel_02_sm.webp",
        "assets/carousel/carousel_02_sm.webp 768w, assets/carousel/carousel_02_lg.webp 1200w, assets/carousel/carousel_02_xxl.webp 2000w",
        "Get your style on point with our fashionable collection",
        "Shop our diverse range of streetwear clothing for both men and women and find your perfect look",
        "Shop now",
        "/store/products"),
        new("carousel/carousel_03_sm.webp",
        "assets/carousel/carousel_03_sm.webp 768w, assets/carousel/carousel_03_lg.webp 1200w, assets/carousel/carousel_03_xxl.webp 2000w",
        "Find your unique style with our chic collection",
        "From bold statement pieces to classic staples, our collection has everything you need to express yourself",
        "Shop for women's now",
        "/store/products?CategoryId=c944566b-92e6-46ec-bc71-248174b5c6de"),
        new("assets/carousel/carousel_01_sm.webp",
        "assets/carousel/carousel_01_sm.webp 768w, assets/carousel/carousel_01_lg.webp 1200w, assets/carousel/carousel_01_xxl.webp 2000w",
        "Elevate your style with our trendy collection",
        "Discover the latest streetwear trends for men and upgrade your wardrobe game",
        "Shop for men's now",
        "/store/products?CategoryId=b80976e9-29e7-4d9b-9ce4-23e3c5c00af9")
    };

    List<ServiceItem> _serviceItems = new()
    {
        new("fa-solid fa-dolly",
        "Daily new arrivals",
        "Stay up-to-date with the latest fashion trends with fresh arrivals added daily"),
        new("fa-regular fa-face-smile-beam",
        "Satisfaction guarantee",
        "Your satisfaction is our top priority, and we'll work with you to ensure it"),
        new("fa-solid fa-hand-holding-dollar",
        "Easy returns",
        "We make it easy for you to return items, hassle-free within 30 days"),
        new("fa-solid fa-truck-fast",
        "Fast and Free Shipping",
        "Loyal customers enjoy quick and free shipping on all orders"),
    };

    List<PickItem> _pickItems = new()
    {
        new("assets/picks/pick_w200(1).webp",
        "assets/picks/pick_w200(1).webp 200w, assets/picks/pick_w400(1).webp 400w",
        "New arrivals",
        "/store/products?Sort=Newest"),
        new("assets/picks/pick_w200(4).webp",
        "assets/picks/pick_w200(4).webp 200w, assets/picks/pick_w400(4).webp 400w",
        "Best sellers",
        "/store/products?Sort=BestSeller"),
        new("assets/picks/pick_w200(3).webp",
        "assets/picks/pick_w200(3).webp 200w, assets/picks/pick_w400(3).webp 400w",
        "Shop men",
        "/store/products?CategoryId=b80976e9-29e7-4d9b-9ce4-23e3c5c00af9"),
        new("assets/picks/pick_w200(2).webp",
        "assets/picks/pick_w200(2).webp 200w, assets/picks/pick_w400(2).webp 400w",
        "Shop women",
        "/store/products?CategoryId=c944566b-92e6-46ec-bc71-248174b5c6de"),

    };

    List<StoreProductModel> _featuredProducts = new();

    private async Task LoadFeaturedProducts(int take)
    {
        var result = await Sender.Send(new GetFeaturedProductsQuery() { Take = take });
        _featuredProducts = result.Data.ToList();
    }

    protected async override Task OnInitializedAsync()
    {
        await LoadFeaturedProducts(4);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jSRuntime.InvokeVoidAsync("window.addLandingCarouselIntersectionObserver");
        }
    }
}
