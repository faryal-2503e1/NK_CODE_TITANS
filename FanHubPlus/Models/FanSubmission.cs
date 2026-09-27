using System.ComponentModel.DataAnnotations;

public class FanSubmission
{
    [Key]
    public int SubmissionId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [MaxLength(500)]
    public string? MediaUrl { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending";

    public DateTime SubmittedAt { get; set; } = DateTime.Now;
}