using System.Linq.Expressions;
using Backend_API.Entities.OrderAggregate;

namespace Core.Specifications;

public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
{
    public OrdersWithItemsAndOrderingSpecification(string email) : base(order => order.BuyerEmail == email)
    {
        AddInclude(order => order.OrderItems);
        AddInclude(order => order.DeliveryMethod);
        AddOrderByDescending(order => order.OrderDate);
    }
    
    public OrdersWithItemsAndOrderingSpecification(int id, string email) : base(order => order.Id == id && order.BuyerEmail == email)
    {
        AddInclude(order => order.OrderItems);
        AddInclude(order => order.DeliveryMethod);
    }
}