using System.ComponentModel.DataAnnotations;

namespace Backend_API.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}