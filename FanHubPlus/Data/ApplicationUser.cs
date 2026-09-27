using Microsoft.AspNetCore.Identity;
using FanHubPlus.Models;

namespace FanHubPlus.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public string? ProfilePictureUrl { get; set; }

        // Fan Hub Plus Navigation Properties
        public ICollection<Bookmark> Bookmarks { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Share> Shares { get; set; }
        public ICollection<UserFavoriteCategory> UserFavoriteCategories { get; set; }
        public ICollection<UserPreference> UserPreferences { get; set; }
        public ICollection<UserActivity> UserActivities { get; set; }
        public ICollection<FanSubmission> FanSubmissions { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<ChatbotQuery> ChatbotQueries { get; set; }
        public ICollection<MediaRating> MediaRatings { get; set; }
    }
}