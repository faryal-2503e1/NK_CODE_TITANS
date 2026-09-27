using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MediaRating
{
    [Key]
    public int RatingId { get; set; }

    public int MediaId { get; set; }
    [ForeignKey("MediaId")]
    public MediaItem MediaItem { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}