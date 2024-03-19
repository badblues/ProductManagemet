using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

[Table("Link")]
public class Link
{
    [Key]
    [Column(Order = 1)]
    [Required]
    public long UpProductId { get; set; }

    [Key]
    [Column(Order = 2)]
    [Required]
    public long ProductId { get; set; }

    [Required]
    public int Count { get; set; }

    public virtual Product? UpProduct { get; set; }

    public virtual Product? Product { get; set; }
}
