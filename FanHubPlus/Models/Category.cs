using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } // Anime, Gaming, Movies, TV Shows, K-Pop, Comics, Manga, Cosplay

    [MaxLength(500)]
    public string? Description { get; set; }

    public ICollection<Content> Contents { get; set; }
    public ICollection<CharacterProfile> CharacterProfiles { get; set; }
    public ICollection<MerchandiseItem> MerchandiseItems { get; set; }
    public ICollection<EventCategory> EventCategories { get; set; }
}
