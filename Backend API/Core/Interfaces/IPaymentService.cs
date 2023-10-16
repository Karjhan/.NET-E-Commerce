using Backend_API.Entities;
using Backend_API.Entities.OrderAggregate;

namespace Core.Interfaces;

public interface IPaymentService
{
    Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);

    Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
    
    Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
}