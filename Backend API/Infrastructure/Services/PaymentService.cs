using Backend_API.Entities;
using Backend_API.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Order = Backend_API.Entities.OrderAggregate.Order;
using Product = Backend_API.Entities.Product;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }
    
    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
        CustomerBasket basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null)
        {
            return null;
        }
        decimal shippingPrice = 0m;
        if (basket.DeliveryMethodId.HasValue)
        {
            DeliveryMethod deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync((int)basket.DeliveryMethodId);
            shippingPrice = deliveryMethod.Price;
        }
        foreach (var item in basket.Items)
        {
            Product productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }
        PaymentIntentService service = new PaymentIntentService();
        PaymentIntent intent;
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions
            {
                Amount = (long)basket.Items.Sum(item => item.Quantity*(item.Price*100))+(long)shippingPrice*100,
                Currency = "usd",
                PaymentMethodTypes = new List<string>(){"card"}
            };
            intent = await service.CreateAsync(options);
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            PaymentIntentUpdateOptions options = new PaymentIntentUpdateOptions
            {
                Amount = (long)basket.Items.Sum(item => item.Quantity * (item.Price * 100)) + (long)shippingPrice * 100,
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }
        await _basketRepository.UpdateBasketAsync(basket);
        return basket;
    }

    public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
        if (order == null)
        {
            return null;
        }
        order.Status = OrderStatus.PaymentFailed;
        await _unitOfWork.Complete();
        return order;
    }

    public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
    {
        var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
        var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecification(spec);
        if (order == null)
        {
            return null;
        }
        order.Status = OrderStatus.PaymentReceived;
        await _unitOfWork.Complete();
        return order;
    }
}