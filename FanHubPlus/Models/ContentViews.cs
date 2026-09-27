using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ContentViews
{
    [Key]
    public int ViewId { get; set; }

    public int ContentId { get; set; }
    [ForeignKey("ContentId")]
    public Content Content { get; set; }

    public string UserId { get; set; }
    public DateTime ViewedAt { get; set; } = DateTime.Now;
}