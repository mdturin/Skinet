using Core.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderByPaymentIntentIdWithItemsSpecification : BaseSpecification<Order>
{
    public OrderByPaymentIntentIdWithItemsSpecification(string paymentIntentId)
        : base(o => o.PaymentIntentId == paymentIntentId)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
    }
}