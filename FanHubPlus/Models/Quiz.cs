using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Quiz
{
    [Key]
    public int QuizId { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<QuizQuestion> Questions { get; set; }
    public ICollection<QuizAttempt> Attempts { get; set; }
}
