using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IBulkRepository<Order> _orderContext;
        private readonly IBulkRepository<DeliveryMethod> _dmContext;
        private readonly IBulkRepository<Product> _productContext;
        private readonly IBasketRepository _basketContext;

        public OrdersService(
            IBulkRepository<Order> orderContext,
            IBulkRepository<DeliveryMethod> dmContext,
            IBulkRepository<Product> productContext,
            IBasketRepository basketContext)
        {
            _orderContext = orderContext;
            _dmContext = dmContext;
            _productContext = productContext;
            _basketContext = basketContext;
        }

        public async Task<Order> CreateOrderAsync(
            string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketContext.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            foreach(var item in basket.BasketItems)
            {
                var productItem = await _productContext.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _dmContext.GetByIdAsync(deliveryMethodId);

            var subTotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal);
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
