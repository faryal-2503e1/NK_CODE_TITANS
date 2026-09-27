using System.ComponentModel.DataAnnotations;

public class ChatbotQuery
{
    [Key]
    public int QueryId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string Message { get; set; }

    [Required]
    public string Response { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}