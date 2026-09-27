
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class CharacterProfile
{
    [Key]
    public int CharacterId { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; }

    public string? Bio { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }
}