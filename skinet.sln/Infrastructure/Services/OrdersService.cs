using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrdersService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketContext;

    public OrdersService(IBasketRepository basketContext, IUnitOfWork unitOfWork)
    {
        _basketContext = basketContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order> CreateOrderAsync(
        string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        var basket = await _basketContext.GetBasketAsync(basketId);
        var items = new List<OrderItem>();
        foreach(var item in basket.BasketItems)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        var deliveryMethod = await _unitOfWork
            .Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        var subTotal = items.Sum(item => item.Price * item.Quantity);
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subTotal);

        _unitOfWork.Repository<Order>().Add(order);

        var result = await _unitOfWork.Complete();

        if(result <= 0) return null;

        await _basketContext.DeleteBasketAsync(basketId);

        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return _unitOfWork.Repository<Order>().ListAsync(spec);
    }
}
