using System.ComponentModel.DataAnnotations;

public class Tag
{
    [Key]
    public int TagId { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    public ICollection<ContentTag> ContentTags { get; set; }
    public ICollection<MediaTag> MediaTags { get; set; }
}