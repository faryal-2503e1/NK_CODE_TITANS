using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EventChatMessage
{
    [Key]
    public int MessageId { get; set; }

    public int EventId { get; set; }
    [ForeignKey("EventId")]
    public Event Event { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; }

    [Required, MaxLength(100)]
    public string UserName { get; set; }

    [Required, MaxLength(500)]
    public string Message { get; set; }

    public DateTime SentAt { get; set; } = DateTime.Now;
}
