using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MerchandiseViews
{
    [Key]
    public int ViewId { get; set; }

    public int ItemId { get; set; }
    [ForeignKey("ItemId")]
    public MerchandiseItem MerchandiseItem { get; set; }

    public string UserId { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.Now;
}