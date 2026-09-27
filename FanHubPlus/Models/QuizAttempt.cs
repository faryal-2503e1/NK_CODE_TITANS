using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class QuizAttempt
{
    [Key]
    public int AttemptId { get; set; }

    public int QuizId { get; set; }
    [ForeignKey("QuizId")]
    public Quiz Quiz { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; }

    public int Score { get; set; }
    public int TotalQuestions { get; set; }
    public double PercentageScore { get; set; }

    public DateTime AttemptedAt { get; set; } = DateTime.Now;
}
