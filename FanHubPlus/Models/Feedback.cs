using System.ComponentModel.DataAnnotations;

public class Feedback
{
    [Key]
    public int FeedbackId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(50)]
    public string Type { get; set; } // bug, suggestion, query

    [Required]
    public string Message { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Open";

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}