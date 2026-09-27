using System.ComponentModel.DataAnnotations;

public class ChatbotFaq
{
    [Key]
    public int FaqId { get; set; }

    [Required, MaxLength(300)]
    public string Question { get; set; }

    [Required]
    public string Answer { get; set; }
}