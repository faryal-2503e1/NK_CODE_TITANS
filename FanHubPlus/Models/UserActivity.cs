using System.ComponentModel.DataAnnotations;

public class UserActivity
{
    [Key]
    public int ActivityId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(200)]
    public string ActionDescription { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}