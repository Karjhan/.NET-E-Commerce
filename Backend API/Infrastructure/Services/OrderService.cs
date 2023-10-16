using Backend_API.Entities;
using Backend_API.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly IBasketRepository _basketRepository;

    public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository)
    {
        _unitOfWork = unitOfWork;
        _basketRepository = basketRepository;
    }

    public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        CustomerBasket basket = await _basketRepository.GetBasketAsync(basketId);
        List<OrderItem> items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            Product productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            ProductItemOrdered itemOrdered = new ProductItemOrdered(productItem.Id,productItem.Name,productItem.PictureURL);
            OrderItem orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }
        DeliveryMethod deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        decimal subtotal = items.Sum(item => item.Price * item.Quantity);
        OrderByPaymentIntentIdSpecification specification = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
        Order order = await _unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);
        if (order != null)
        {
            order.ShipToAddress = shippingAddress;
            order.DeliveryMethod = deliveryMethod;
            order.Subtotal = subtotal;
            _unitOfWork.Repository<Order>().Update(order);
        }
        else
        {
            order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal, basket.PaymentIntentId);
            _unitOfWork.Repository<Order>().Add(order);
        }
        var result = await _unitOfWork.Complete();
        if (result <= 0)
        {
            return null;
        }
        // await _basketRepository.DeleteBasketAsync(basketId);
        return order;
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var specification = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListAllAsync(specification);
    }

    public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
    {
        var specification = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }
}