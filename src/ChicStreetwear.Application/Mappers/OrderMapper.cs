using ChicStreetwear.Domain.Aggregates.Order;
using ChicStreetwear.Domain.Aggregates.Order.Entities;
using ChicStreetwear.Domain.Aggregates.Order.ValueObjects;
using ChicStreetwear.Domain.ValueObjects;
using ChicStreetwear.Shared.Models.Order;
using Riok.Mapperly.Abstractions;

namespace ChicStreetwear.Application.Common.Mappers;

[Mapper]
internal static partial class OrderMapper
{
    internal static partial OrderModel ToOrderModel(this Order order);
    private static partial AddressModel ToAddressModel(this Address address);
    private static partial OrderProductModel ToOrderProductModel(this OrderProduct product);
    private static decimal ToDecimal(Money money) => money.Amount;
    private static partial OrderProductAttributeModel ToOrderProductAttributeModel(this OrderProductAttribute attribute);

    internal static Address ToAddress(this AddressModel model)
        => Address.New(model.FirstName, model.LastName, model.FullName, model.Email, model.PhoneNumber, model.AddressLine1, model.AddressLine2, model.Country, model.City, model.State, model.PostalCode);

    internal static OrderProduct ToOrderProduct(this OrderProductModel model)
        => OrderProduct.New(model.ProductId, model.Name, model.Description, model.Quantity, model.Price, model.SellerId, model.VariationId, model.Attributes.Select(a => OrderProductAttribute.New(a.Name, a.Value)).ToList());
}