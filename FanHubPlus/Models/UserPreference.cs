using System.ComponentModel.DataAnnotations;

public class UserPreference
{
    [Key]
    public int PreferenceId { get; set; }

    [Required]
    public string UserId { get; set; }

    [MaxLength(50)]
    public string ThemeMode { get; set; } // Dark, Light

    [MaxLength(20)]
    public string FontSize { get; set; }

    public bool EmailNotifications { get; set; } = true;
}