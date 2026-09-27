using System.ComponentModel.DataAnnotations;

public class ActivityLog
{
    [Key]
    public int LogId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(100)]
    public string ActivityType { get; set; }

    [Required, MaxLength(500)]
    public string Details { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.Now;
}