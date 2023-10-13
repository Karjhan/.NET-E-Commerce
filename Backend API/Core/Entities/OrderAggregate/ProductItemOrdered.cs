namespace Backend_API.Entities.OrderAggregate;

public class ProductItemOrdered
{
    public int ProductItemId { get; set; }

    public string ProductName { get; set; }

    public string PictureURL { get; set; }

    public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
    {
        ProductItemId = productItemId;
        ProductName = productName;
        PictureURL = pictureUrl;
    }

    public ProductItemOrdered()
    {
        
    }
}