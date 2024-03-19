using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("Product")]
public class Product
{
    [Key]
    [Required]
    public long Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    [Required]
    public float Price { get; set; }

    public ICollection<Link> ProductsBelow { get; set; } = new List<Link>();

    public ICollection<Link> UpProducts { get; set; } = new List<Link>();
}
