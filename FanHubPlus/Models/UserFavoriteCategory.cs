using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserFavoriteCategory
{
    [Key]
    public int FavoriteId { get; set; }

    [Required]
    public string UserId { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}