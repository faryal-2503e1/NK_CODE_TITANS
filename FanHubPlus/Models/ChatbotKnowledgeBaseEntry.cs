using System.ComponentModel.DataAnnotations;

public class ChatbotKnowledgeBaseEntry
{
    [Key]
    public int EntryId { get; set; }

    [Required, MaxLength(200)]
    public string Topic { get; set; }

    [Required]
    public string Content { get; set; }
}