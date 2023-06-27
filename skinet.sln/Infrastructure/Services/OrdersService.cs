using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrdersService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymentService _paymentService;
    private readonly IBasketRepository _basketContext;

    public OrdersService(
        IBasketRepository basketContext, IUnitOfWork unitOfWork, IPaymentService paymentService)
    {
        _basketContext = basketContext;
        _unitOfWork = unitOfWork;
        _paymentService = paymentService;
    }

    public async Task<Order> CreateOrderAsync(
        string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        var basket = await _basketContext.GetBasketAsync(basketId);
        var items = new List<OrderItem>();
        foreach (var item in basket.BasketItems)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        var deliveryMethod = await _unitOfWork
            .Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        var subTotal = items.Sum(item => item.Price * item.Quantity);

        string paymentIntentId = basket.PaymentIntentId;
        var spec = new OrderByPaymentIntentIdWithItemsSpecification(paymentIntentId);
        var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

        if (existingOrder != null)
        {
            _unitOfWork.Repository<Order>().Delete(existingOrder);
            await _paymentService.CreateOrUpdatePaymentIntent(paymentIntentId);
        }

        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod,
                              subTotal, paymentIntentId);

        _unitOfWork.Repository<Order>().Add(order);

        var result = await _unitOfWork.Complete();

        if (result <= 0) return null;

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
