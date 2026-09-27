using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PollVote
{
    [Key]
    public int VoteId { get; set; }

    public int PollId { get; set; }
    [ForeignKey("PollId")]
    public Poll Poll { get; set; }

    public int OptionId { get; set; }
    [ForeignKey("OptionId")]
    public PollOption Option { get; set; }

    [Required, MaxLength(450)]
    public string UserId { get; set; }

    public DateTime VotedAt { get; set; } = DateTime.Now;
}
