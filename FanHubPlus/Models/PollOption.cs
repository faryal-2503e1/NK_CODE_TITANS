using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PollOption
{
    [Key]
    public int OptionId { get; set; }

    public int PollId { get; set; }
    [ForeignKey("PollId")]
    public Poll Poll { get; set; }

    [Required, MaxLength(150)]
    public string OptionText { get; set; }

    public ICollection<PollVote> Votes { get; set; }
}
