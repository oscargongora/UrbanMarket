using ChicStreetwear.Domain.Aggregates.Cart.Entities;
using ChicStreetwear.Domain.Common;
using ChicStreetwear.Domain.Common.Interfaces;
using ChicStreetwear.Domain.Errors;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared;

namespace ChicStreetwear.Domain.Aggregates.Cart;
public sealed class Cart : EntityBase, IAggregateRoot
{
    private readonly List<CartProduct> _products = new();
    public IReadOnlyList<CartProduct> Products => _products.AsReadOnly();
    public Money Total { get; private set; } = Money.New(0);
    public Guid? CustomerId { get; private set; }

    private Cart(Guid id, Guid? customerId) : base(id)
    {
        CustomerId = customerId;
    }

    public static Cart New(Guid? customerId)
    {
        return new Cart(Guid.NewGuid(), customerId);
    }

    public Result<Cart> AddProduct(CartProduct product)
    {
        if (product.Price.Amount < 0)
            return MoneyErrors.InvalidAmount(nameof(Cart.AddProduct));
        if (product.Quantity < 1)
            return CartErrors.InvalidProductQuantity(nameof(Cart.AddProduct));
        _products.Add(product);
        Total = Money.New(Total.Amount + (product.Quantity * product.Price.Amount));
        return Result<Cart>.Succeeded(this);
    }

    public Result<Cart> UpdateProducts(List<(Guid id, CartProduct product)> products)
    {
        foreach (var (id, product) in products)
        {
            var result = UpdateProduct(id, product.Name, product.Description, product.Quantity, product.Price.Amount);

            if (result.HasErrors) return result;
        }
        return Result<Cart>.Succeeded(this);
    }

    public Result<Cart> UpdateProduct(Guid cartProductId, int quantity)
    {
        if (quantity < 1)
            return CartErrors.InvalidProductQuantity(nameof(Cart.UpdateProduct));

        var _product = _products.FirstOrDefault(_p => _p.Id == cartProductId);
        if (_product is null)
            return CartErrors.ProductNotFound;

        Total = Money.New(Total.Amount - (_product.Quantity * _product.Price.Amount) + (quantity * _product.Price.Amount));

        _product.Update(_product.Name, _product.Description, quantity, _product.Price.Amount);

        return Result<Cart>.Succeeded(this);
    }

    public Result<Cart> UpdateProduct(Guid productId, string name, string description, int quantity, decimal price)
    {
        if (price < 0)
            return MoneyErrors.InvalidAmount(nameof(Cart.UpdateProduct));

        if (quantity < 1)
            return CartErrors.InvalidProductQuantity(nameof(Cart.UpdateProduct));

        var _product = _products.FirstOrDefault(_p => _p.Id == productId);
        if (_product is null)
            return CartErrors.ProductNotFound;

        Total = Money.New(Total.Amount - (_product.Quantity * _product.Price.Amount) + (quantity * price));

        _product.Update(name, description, quantity, price);

        return Result<Cart>.Succeeded(this);
    }

    public Result<Cart> RemoveProduct(Guid cartProductId)
    {
        var _product = _products.FirstOrDefault(_p => _p.Id.Equals(cartProductId));

        if (_product is null)
            return CartErrors.ProductNotFound;

        Total = Money.New(Total.Amount - (_product.Quantity * _product.Price.Amount));

        _products.Remove(_product);

        return Result<Cart>.Succeeded(this);
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Cart() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
