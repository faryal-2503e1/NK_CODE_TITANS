using System.ComponentModel.DataAnnotations.Schema;

public class ContentTag
{
    public int ContentId { get; set; }
    [ForeignKey("ContentId")]
    public Content Content { get; set; }

    public int TagId { get; set; }
    [ForeignKey("TagId")]
    public Tag Tag { get; set; }
}