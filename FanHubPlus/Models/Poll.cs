using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Poll
{
    [Key]
    public int PollId { get; set; }

    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }

    [Required, MaxLength(200)]
    public string Title { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public ICollection<PollOption> Options { get; set; }
    public ICollection<PollVote> Votes { get; set; }
}
