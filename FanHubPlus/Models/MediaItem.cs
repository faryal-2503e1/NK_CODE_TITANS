using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MediaItem
{
    [Key]
    public int MediaId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? FileUrl { get; set; }

    [MaxLength(50)]
    public string? MediaType { get; set; } // Video, Audio, Trailer, Gallery

    public DateTime UploadedAt { get; set; } = DateTime.Now;

    // Admin categorisation and tagging
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    public ICollection<MediaTag> MediaTags { get; set; }

    public ICollection<MediaRating> MediaRatings { get; set; }
}