using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FanHubPlus.Models; // Ensure this matches your models namespace if they are in a separate folder

namespace FanHubPlus.Data
{
    public class FanHubContext : IdentityDbContext<ApplicationUser>
    {
        public FanHubContext(DbContextOptions<FanHubContext> options)
            : base(options) { }

        // 26 Domain Tables DbSets
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<MediaTag> MediaTags { get; set; }
        public DbSet<MediaItem> MediaItems { get; set; }
        public DbSet<MediaRating> MediaRatings { get; set; }
        public DbSet<CharacterProfile> CharacterProfiles { get; set; }
        public DbSet<FeaturedArticle> FeaturedArticles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventTicketLink> EventTicketLinks { get; set; }
        public DbSet<MerchandiseItem> MerchandiseItems { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Share> Shares { get; set; }
        public DbSet<UserFavoriteCategory> UserFavoriteCategories { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<FanSubmission> FanSubmissions { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ChatbotQuery> ChatbotQueries { get; set; }
        public DbSet<ChatbotFaq> ChatbotFaqs { get; set; }
        public DbSet<ChatbotKnowledgeBaseEntry> ChatbotKnowledgeBaseEntries { get; set; }
        public DbSet<ContentViews> ContentViews { get; set; }
        public DbSet<MerchandiseViews> MerchandiseViews { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        // Fan Quiz System
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }

        // Live Polls & Watch Parties
        public DbSet<Poll> Polls { get; set; }
        public DbSet<PollOption> PollOptions { get; set; }
        public DbSet<PollVote> PollVotes { get; set; }

        // Live Event Chat
        public DbSet<EventChatMessage> EventChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Composite Primary Keys for Many-to-Many Bridge Tables
            builder.Entity<ContentTag>()
                .HasKey(ct => new { ct.ContentId, ct.TagId });

            builder.Entity<MediaTag>()
                .HasKey(mt => new { mt.MediaId, mt.TagId });

            builder.Entity<EventCategory>()
                .HasKey(ec => new { ec.EventId, ec.CategoryId });

            // Additional fluent configurations or delete behaviors can be added here if needed
        }
    }
}
