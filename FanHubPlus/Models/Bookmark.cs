using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Bookmark
{
    [Key]
    public int BookmarkId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(50)]
    public string ItemType { get; set; } // Content, Character, Media, Merchandise

    public int? ContentId { get; set; }
    [ForeignKey("ContentId")]
    public Content Content { get; set; }

    public int? CharacterId { get; set; }
    [ForeignKey("CharacterId")]
    public CharacterProfile Character { get; set; }

    public int? MediaItemId { get; set; }
    [ForeignKey("MediaItemId")]
    public MediaItem MediaItem { get; set; }

    public int? MerchandiseItemId { get; set; }
    [ForeignKey("MerchandiseItemId")]
    public MerchandiseItem MerchandiseItem { get; set; }

    [MaxLength(500)]
    public string? NoteText { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
