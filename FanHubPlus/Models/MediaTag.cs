using System.ComponentModel.DataAnnotations.Schema;

public class MediaTag
{
    public int MediaId { get; set; }
    [ForeignKey("MediaId")]
    public MediaItem MediaItem { get; set; }

    public int TagId { get; set; }
    [ForeignKey("TagId")]
    public Tag Tag { get; set; }
}
