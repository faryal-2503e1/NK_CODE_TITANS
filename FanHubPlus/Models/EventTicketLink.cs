using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EventTicketLink
{
    [Key]
    public int TicketLinkId { get; set; }

    public int EventId { get; set; }
    [ForeignKey("EventId")]
    public Event Event { get; set; }

    [Required, MaxLength(100)]
    public string PlatformName { get; set; }

    [Required, MaxLength(500)]
    public string Url { get; set; }
}