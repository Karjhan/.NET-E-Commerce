using System.Linq.Expressions;
using Backend_API.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
{
    public OrderByPaymentIntentIdSpecification(string paymentIntentId) : base(order => order.PaymentIntentId == paymentIntentId)
    {
        
    }
}