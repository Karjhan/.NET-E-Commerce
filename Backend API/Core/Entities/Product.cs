using System.ComponentModel.DataAnnotations;

namespace Backend_API.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
}