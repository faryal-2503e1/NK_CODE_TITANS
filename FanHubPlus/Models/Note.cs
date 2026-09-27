using System.ComponentModel.DataAnnotations;

public class Note
{
    [Key]
    public int NoteId { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}