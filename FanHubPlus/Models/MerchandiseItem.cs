using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MerchandiseItem
{
    [Key]
    public int ItemId { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(50)]
    public string? Tag { get; set; } // Limited Edition, Pre-Order, Collectible

    public bool IsUpcoming { get; set; }

    public ICollection<MerchandiseViews> MerchandiseViews { get; set; }
}