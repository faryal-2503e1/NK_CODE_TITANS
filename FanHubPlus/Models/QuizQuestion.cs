using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class QuizQuestion
{
    [Key]
    public int QuestionId { get; set; }

    public int QuizId { get; set; }
    [ForeignKey("QuizId")]
    public Quiz Quiz { get; set; }

    [Required, MaxLength(500)]
    public string QuestionText { get; set; }

    [Required, MaxLength(200)]
    public string OptionA { get; set; }

    [Required, MaxLength(200)]
    public string OptionB { get; set; }

    [Required, MaxLength(200)]
    public string OptionC { get; set; }

    [Required, MaxLength(200)]
    public string OptionD { get; set; }

    // Value "A", "B", "C" ya "D" store hogi
    [Required, MaxLength(1)]
    public string CorrectOption { get; set; }
}
