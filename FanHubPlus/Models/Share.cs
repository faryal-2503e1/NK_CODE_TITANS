using System.ComponentModel.DataAnnotations;

public class Share
{
    [Key]
    public int ShareId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(50)]
    public string Platform { get; set; }

    [Required, MaxLength(500)]
    public string SharedUrl { get; set; }

    public DateTime SharedAt { get; set; } = DateTime.Now;
}