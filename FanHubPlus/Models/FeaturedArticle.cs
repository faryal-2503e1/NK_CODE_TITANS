using System.ComponentModel.DataAnnotations;

public class FeaturedArticle
{
    [Key]
    public int ArticleId { get; set; }

    [Required, MaxLength(250)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public DateTime PublishedAt { get; set; } = DateTime.Now;
}