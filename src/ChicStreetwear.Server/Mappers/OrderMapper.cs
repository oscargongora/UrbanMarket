using ChicStreetwear.Application.Orders.Commands;
using ChicStreetwear.Application.Orders.Queries;
using Riok.Mapperly.Abstractions;
using static ChicStreetwear.Server.Requests.Requests.Order;

namespace ChicStreetwear.Server.Mappers;

[Mapper]
internal static partial class OrderMapper
{
    internal static partial CreateOrderCommand ToCommand(this CreateOrderRequest request);
    internal static partial UpdateOrderCommand ToCommand(this UpdateOrderRequest request);
    internal static partial GetOrdersQuery ToQuery(this GetOrdersRequest request);
}