using System.ComponentModel.DataAnnotations;

public class Event
{
    [Key]
    public int EventId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    public string? Description { get; set; }
    public DateTime EventDate { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(300)]
    public string? LocationCoordinates { get; set; }

    public ICollection<EventCategory> EventCategories { get; set; }
    public ICollection<EventTicketLink> EventTicketLinks { get; set; }
}