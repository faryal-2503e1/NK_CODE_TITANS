using System.ComponentModel.DataAnnotations.Schema;

public class EventCategory
{
    public int EventId { get; set; }
    [ForeignKey("EventId")]
    public Event Event { get; set; }

    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
}