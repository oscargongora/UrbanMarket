using ChicStreetwear.Shared.Models.Order;
using static ChicStreetwear.Api.Requests;

namespace ChicStreetwear.Server.Requests;

public static partial class Requests
{
    public static class Order
    {
        public sealed class CreateOrderRequest : OrderModel { }
        public sealed class UpdateOrderRequest : OrderModel { }
        public sealed class GetOrdersRequest : GetRequestBase { }
    }
}