using System.ComponentModel.DataAnnotations;

public class ProfileEditViewModel
{
    [MaxLength(150)]
    public string? FullName { get; set; }

    [MaxLength(300)]
    public string? Address { get; set; }

    public string? CurrentProfilePictureUrl { get; set; }
}
