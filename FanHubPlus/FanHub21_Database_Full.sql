-- =========================================================
-- FanHub21 Complete SQL Server Database Script (Schema + Data)
-- Generated: 2026-09-27 20:27:56
-- =========================================================

USE [master];
GO
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'FanHub21') BEGIN
    CREATE DATABASE [FanHub21];
END
GO
USE [FanHub21];
GO

-- ---------------------------------------------------------
-- Table Structure for [__EFMigrationsHistory]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[__EFMigrationsHistory] (
    [MigrationId] [nvarchar]  NOT NULL,
    [ProductVersion] [nvarchar]  NOT NULL
);
END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260924095837_MergedModels', N'8.0.23');
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260926142055_AddQuizSystem', N'8.0.23');
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260927111334_myyy', N'8.0.23');
GO

-- ---------------------------------------------------------
-- Table Structure for [ActivityLogs]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ActivityLogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ActivityLogs] (
    [LogId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ActivityType] [nvarchar]  NOT NULL,
    [Details] [nvarchar]  NOT NULL,
    [Timestamp] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetRoleClaims]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [RoleId] [nvarchar]  NOT NULL,
    [ClaimType] [nvarchar]  NULL,
    [ClaimValue] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetRoles]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] [nvarchar]  NOT NULL,
    [Name] [nvarchar]  NULL,
    [NormalizedName] [nvarchar]  NULL,
    [ConcurrencyStamp] [nvarchar]  NULL
);
END
GO

INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'8bb28dc4-4106-4cab-9913-e4b7ba83893c', N'Customer', N'CUSTOMER', NULL);
INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'c13b9ab6-45b5-4552-b3be-f6eebcec4c9e', N'Admin', N'ADMIN', NULL);
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetUserClaims]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ClaimType] [nvarchar]  NULL,
    [ClaimValue] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetUserLogins]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] [nvarchar]  NOT NULL,
    [ProviderKey] [nvarchar]  NOT NULL,
    [ProviderDisplayName] [nvarchar]  NULL,
    [UserId] [nvarchar]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetUserRoles]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] [nvarchar]  NOT NULL,
    [RoleId] [nvarchar]  NOT NULL
);
END
GO

INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'42d7a79f-0725-42cf-bd1f-85fdc4c6e054', N'8bb28dc4-4106-4cab-9913-e4b7ba83893c');
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'71baae65-44ac-4ad3-9b4c-390a6e31eb53', N'8bb28dc4-4106-4cab-9913-e4b7ba83893c');
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'88ad1b1f-360f-4266-afc8-c952aefc2141', N'8bb28dc4-4106-4cab-9913-e4b7ba83893c');
INSERT INTO [AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd1f26ac5-631f-4fd9-8f35-378cd68c8944', N'c13b9ab6-45b5-4552-b3be-f6eebcec4c9e');
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetUsers]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] [nvarchar]  NOT NULL,
    [FullName] [nvarchar]  NULL,
    [Address] [nvarchar]  NULL,
    [ProfilePictureUrl] [nvarchar]  NULL,
    [UserName] [nvarchar]  NULL,
    [NormalizedUserName] [nvarchar]  NULL,
    [Email] [nvarchar]  NULL,
    [NormalizedEmail] [nvarchar]  NULL,
    [EmailConfirmed] [bit]  NOT NULL,
    [PasswordHash] [nvarchar]  NULL,
    [SecurityStamp] [nvarchar]  NULL,
    [ConcurrencyStamp] [nvarchar]  NULL,
    [PhoneNumber] [nvarchar]  NULL,
    [PhoneNumberConfirmed] [bit]  NOT NULL,
    [TwoFactorEnabled] [bit]  NOT NULL,
    [LockoutEnd] [datetimeoffset]  NULL,
    [LockoutEnabled] [bit]  NOT NULL,
    [AccessFailedCount] [int]  NOT NULL
);
END
GO

INSERT INTO [AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'42d7a79f-0725-42cf-bd1f-85fdc4c6e054', NULL, NULL, NULL, N'irmah@gmail.com', N'IRMAH@GMAIL.COM', N'irmah@gmail.com', N'IRMAH@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEAD2Mm9G32BDbrGZ3xJzFV42t72Dlqp9yDJFsvwW+Ds7SWbqGeEGUbMmar8yEKfm8A==', N'UA4QNHLGVSOZAGWKRAKP322BNMQVT7XN', N'9f685e37-311e-44ee-af82-7d38a7f9b57a', NULL, 0, 0, NULL, NULL, 0);
INSERT INTO [AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'71baae65-44ac-4ad3-9b4c-390a6e31eb53', NULL, NULL, NULL, N'nadeemirmah@gmail.com', N'NADEEMIRMAH@GMAIL.COM', N'nadeemirmah@gmail.com', N'NADEEMIRMAH@GMAIL.COM', NULL, N'AQAAAAIAAYagAAAAEFjAD4svoImynHwEPDOOVuNqytS0lkHvpDtKdPqGUslxKT7KWqVJVtX1S5OqGzMH/g==', N'SCODAIORVJTM4UHTZQ3GUVTVMZW33W4Z', N'9c34d747-b923-4a83-a9f3-69edbead9089', NULL, 0, 0, NULL, NULL, 0);
INSERT INTO [AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'88ad1b1f-360f-4266-afc8-c952aefc2141', NULL, NULL, NULL, N'azeemmoiz522@gmail.com', N'AZEEMMOIZ522@GMAIL.COM', N'azeemmoiz522@gmail.com', N'AZEEMMOIZ522@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEBwuxeuSFzols27Ztm4Ug/M7MTTgB/9uFpyyY0q4FZLmOs5/Pg440feGej/N4NUP8w==', N'KZ6OKIWZ6ZRDMTLWXAZ6E47J5NMGKA3G', N'b7863b1a-1021-49ca-8531-f370e14dffea', NULL, 0, 0, NULL, NULL, 0);
INSERT INTO [AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd1f26ac5-631f-4fd9-8f35-378cd68c8944', N'Irmah khan', NULL, NULL, N'admin@demo.com', N'ADMIN@DEMO.COM', N'admin@demo.com', N'ADMIN@DEMO.COM', NULL, N'AQAAAAIAAYagAAAAEMROHoi6FMgMOuYlflslYrm6zs8N4QU/GeTl/w/kSyrFP+BFVMGMVMYCfgX8auSAbQ==', N'FIKR776RU7DS64AQJHORW6CLOJJJGFRT', N'b74d19d6-f081-4baf-af19-cba2b5d33dae', NULL, 0, 0, NULL, NULL, 0);
GO

-- ---------------------------------------------------------
-- Table Structure for [AspNetUserTokens]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] [nvarchar]  NOT NULL,
    [LoginProvider] [nvarchar]  NOT NULL,
    [Name] [nvarchar]  NOT NULL,
    [Value] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [Bookmarks]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bookmarks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Bookmarks] (
    [BookmarkId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ItemType] [nvarchar]  NOT NULL,
    [ContentId] [int]  NULL,
    [CharacterId] [int]  NULL,
    [MediaItemId] [int]  NULL,
    [MerchandiseItemId] [int]  NULL,
    [NoteText] [nvarchar]  NOT NULL,
    [CreatedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

SET IDENTITY_INSERT [Bookmarks] ON;
GO
INSERT INTO [Bookmarks] ([BookmarkId], [UserId], [ItemType], [ContentId], [CharacterId], [MediaItemId], [MerchandiseItemId], [NoteText], [CreatedAt], [ApplicationUserId]) VALUES (2, N'42d7a79f-0725-42cf-bd1f-85fdc4c6e054', N'Content', 5, NULL, NULL, NULL, NULL, N'09/26/2026 19:35:54', NULL);
SET IDENTITY_INSERT [Bookmarks] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [Categories]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories] (
    [CategoryId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar]  NOT NULL,
    [Description] [nvarchar]  NULL
);
END
GO

SET IDENTITY_INSERT [Categories] ON;
GO
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (1, N'Anime', N'Anime series, films and the characters behind them.?');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (2, N'Gaming', N'Video games, esports and gaming culture.');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (3, N'Movies', N'Feature films, premieres and trailers.');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (4, N'TV Shows', N'Television series, seasons and episode news.');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (5, N'K-Pop', N'Korean pop artists, releases and fan culture.');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (6, N'Comics', N'Comic books, graphic novels and creators.');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (7, N'Manga', N'Manga series, chapters and volume releases.');
INSERT INTO [Categories] ([CategoryId], [Name], [Description]) VALUES (8, N'Cosplay', N'Cosplay showcases, meetups and craft guides.');
SET IDENTITY_INSERT [Categories] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [CharacterProfiles]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CharacterProfiles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CharacterProfiles] (
    [CharacterId] [int] IDENTITY(1,1) NOT NULL,
    [CategoryId] [int]  NOT NULL,
    [Name] [nvarchar]  NOT NULL,
    [Bio] [nvarchar]  NULL,
    [ImageUrl] [nvarchar]  NULL
);
END
GO

SET IDENTITY_INSERT [CharacterProfiles] ON;
GO
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (2, 1, N'Kaito Tanaka', N'A strong-willed and determined young hero with a signature spiky hairstyle, striving to protect his world from dark forces.', N'/uploads/characters/7a78a389aa5b4edca118c202924325a7.jpg');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (3, 2, N'Ren Akabane', N'A calm, highly strategic, and powerful combat expert who constantly pushes his limits in the virtual arena.', N'/uploads/characters/62811533d5a146f8acc2b29d0cf9022a.jpg');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (4, 3, N'Sora Hoshino', N'A loyal and brilliant friend who serves as the tactical and tech expert, always keeping the team grounded.', N'/uploads/characters/50e89e1aaa864631ae2ea4cac82599e2.jpg');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (5, 7, N'Master Jin', N'A wise veteran with a mysterious past and unmatched high-tier combat skills, guiding the next generation of fighters.', N'/uploads/characters/9160e2c0193c4f1e84b38a75fb108d1c.jpg');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (6, 4, N'Malakor', N'A formidable villain driven by a complex backstory and deep philosophical motives, believing his actions will bring true balance.', N'/uploads/characters/8d901d11b3654074b95a9f69b76e83de.jpg');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (7, 2, N'Kratos (God of War)', N'The Ghost of Sparta who defeated the Olympian and Norse pantheons.', N'/images/explore/genshin-impact.png');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (8, 3, N'Tony Stark (Iron Man)', N'Genius, billionaire, playboy, philanthropist who saved the universe.', N'/images/explore/spider-man.png');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (9, 4, N'Eleven (Stranger Things)', N'A young girl with powerful psychokinetic abilities protecting Hawkins.', N'/images/explore/stranger-things.png');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (10, 5, N'Jennie Kim (Blackpink)', N'Global icon, lead vocalist and rapper of world-renowned K-Pop group Blackpink.', N'/images/explore/blackpink.png');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (11, 6, N'Batman (Bruce Wayne)', N'The Dark Knight of Gotham City fighting crime with martial arts and technology.', N'/images/explore/spider-man.png');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (12, 7, N'Zoro Roronoa', N'The Master Swordsman of the Straw Hat Pirates wielding three katanas.', N'/images/explore/one-piece.png');
INSERT INTO [CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (13, 8, N'Cyber Neon Cosplay Queen', N'Renowned international cosplayer known for glowing armor and wings.', N'/assets/images/hero/cosplay-hero.png');
SET IDENTITY_INSERT [CharacterProfiles] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [ChatbotFaqs]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatbotFaqs]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ChatbotFaqs] (
    [FaqId] [int] IDENTITY(1,1) NOT NULL,
    [Question] [nvarchar]  NOT NULL,
    [Answer] [nvarchar]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [ChatbotFaqs] ON;
GO
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (1, N'what is this website', N'FanHubPlus is a community hub for fans to explore content, characters, events, and merchandise, and connect with other fans who share the same interests.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (2, N'how do i bookmark something', N'Open any content, character, media or merchandise page and click the Bookmark button. You need to be logged in to save bookmarks.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (3, N'how do i submit fan art', N'Go to Fan Submissions from your dashboard and click "Submit Your Work". Every submission is reviewed before it''s published.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (4, N'how do i create an account', N'Click Register at the top of the page and fill in your email and password to get started.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (5, N'how do i contact support', N'You can reach out through the Contact page, or send us feedback from your dashboard''s Feedback section.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (6, N'how do i change my profile picture', N'Go to your Profile, click Edit Profile, and upload a new picture from the Profile Picture section.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (7, N'How do I create an account on Fan Hub Plus?', N'Click Register on the top navigation bar, enter your name, email, and password, then confirm your email using the link we send you. Once verified, you can log in and start exploring.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (8, N'I forgot my password. How do I reset it?', N'On the login page, click ''Forgot password?'', enter the email address you registered with, and we''ll send you a secure link to set a new password.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (9, N'Do I need an account to browse content?', N'No. Visitors can browse categories, content, character profiles, and merchandise without logging in. You only need an account to bookmark items, leave ratings, take notes, or submit fan content.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (10, N'What fandom categories does the platform cover?', N'Anime, Gaming, Movies, TV Shows, K-Pop, Comics, Manga, and Cosplay. You can browse each category from the homepage or the category menu.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (11, N'How can I search or filter content?', N'Use the search bar to find content by title, or use the filters for category, genre, release year, content type, and popularity. You can also sort results by latest, most popular, or alphabetically.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (12, N'Can I update my profile picture and favorite fandoms?', N'Yes. Go to your Profile page, click Edit, and you can upload a profile picture, change your favorite fandoms, and update your display preferences at any time.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (13, N'What shows up on my personal dashboard?', N'Your dashboard shows a personalized greeting, your recent activity, your favorite fandoms, and the items you''ve bookmarked, all in one place.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (14, N'How do I bookmark an article, character, video, or merchandise item?', N'Open the item you''re interested in and click the bookmark icon. You can view and manage all your saved items later from your Dashboard or the Bookmarks page.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (15, N'Can I add personal notes to a bookmarked item?', N'Yes. Open a bookmarked item and use the Notes option to add, edit, or delete your own private notes about it.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (16, N'How do I rate or review a video, trailer, or audio clip?', N'On the media item''s page, use the star rating or thumbs-up/down control below the player to leave your feedback.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (17, N'Can I filter character profiles by fandom?', N'Yes. On the Character Profiles page, use the category filter to show only characters from a specific fandom, such as Anime or Gaming.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (18, N'How do I find upcoming anime, games, movies, or merchandise releases?', N'Check the Upcoming Releases section under Merchandise or Content. It lists anticipated anime, games, movies, shows, comics, and merchandise drops with tags like ''Pre-Order'' or ''Limited Edition''.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (19, N'Can I submit my own fan content or articles?', N'Yes. Go to Fan Submissions and click ''Submit Content''. Your submission will be reviewed by an admin before it appears publicly on the platform.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (20, N'How long does it take for my fan submission to be approved?', N'Submissions are reviewed by our admin team as quickly as possible. You''ll see the status (pending, approved, or rejected) on your My Submissions page.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (21, N'How do I find fan conventions, meetups, or screenings near me?', N'Open the Events page and either allow location access to see nearby events on the map, or use the city filter in the Event Calendar to browse events by location.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (22, N'Do event listings include ticket links?', N'Yes. Where available, each event page includes a link to the official ticket or registration platform.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (23, N'How do I report a bug or leave feedback?', N'Go to the Feedback page, choose a type (bug, suggestion, or query), describe the issue, and submit. Our admin team reviews all feedback submitted through this form.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (24, N'Can I enable dark mode or change the font size?', N'Yes. Use the accessibility controls in the site header to toggle dark mode or increase/decrease the font size for easier reading.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (25, N'Can I purchase merchandise directly from the website?', N'No. Fan Hub Plus only showcases merchandise for display and discovery. We don''t process orders or payments; the merchandise section is for browsing only.');
INSERT INTO [ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (26, N'What can you (the chatbot) help me with?', N'I can answer questions about the platform, help you find content or categories based on what you''re interested in, and guide you through features like bookmarking, submissions, and events.');
SET IDENTITY_INSERT [ChatbotFaqs] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [ChatbotKnowledgeBaseEntries]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatbotKnowledgeBaseEntries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ChatbotKnowledgeBaseEntries] (
    [EntryId] [int] IDENTITY(1,1) NOT NULL,
    [Topic] [nvarchar]  NOT NULL,
    [Content] [nvarchar]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [ChatbotKnowledgeBaseEntries] ON;
GO
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (1, N'Bookmarks', N'Bookmarks let registered users save any content, character, media item or merchandise item for later. Login is required.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (2, N'Events Management', N'Explore all upcoming global anime conventions, cosplay championships, art exhibitions, and live music festivals with dates and locations.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (3, N'Character Profiles', N'Discover detailed profiles of protagonists, rivals, mentors, and antagonists categorized across anime, gaming, manga, and movies.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (4, N'Fan Submissions', N'Registered users can submit their own custom fan art, cosplay photos, and reviews through the user portal for admin review.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (5, N'Merchandise Store', N'Browse through official collectibles, apparel, action figures, artbooks, and night lamps with secure ordering details.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (6, N'Account Registration', N'New visitors can register with an email address and password. A confirmation link is sent by email, and the account becomes active once verified. No account is required for browsing.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (7, N'Content Explorer and Filters', N'The Content Explorer pulls curated articles, profiles, and media from the database. Users can filter by category, genre, release year, popularity, or content type, and sort by latest, most popular, or alphabetical order.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (8, N'Bookmarking and Notes', N'Registered users can bookmark any article, character profile, video, or merchandise item, and attach a personal note to it. Bookmarks are visible from the user''s dashboard.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (9, N'Fan Submission Workflow', N'Registered users can submit fan-made content or articles. Every submission starts in a Pending state and only becomes publicly visible once an admin approves it from the Fan Submissions panel.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (10, N'Event Discovery', N'Events (conventions, meetups, screenings) can be discovered either through map/GPS-based nearby search or through a city-filterable calendar. Listings may include an external ticket link.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (11, N'Merchandise Showcase', N'Merchandise items are grouped by fandom and category, and tagged as Limited Edition, Pre-Order, or Collectible by the admin. The platform does not support purchasing, checkout, or payments.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (12, N'Feedback Types', N'User feedback is categorized as Bug, Suggestion, or Query when submitted, which helps the admin team triage and respond appropriately.');
INSERT INTO [ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (13, N'Accessibility Features', N'The platform supports a dark mode toggle and font-size adjustment to improve readability, along with breadcrumb navigation across category pages.');
SET IDENTITY_INSERT [ChatbotKnowledgeBaseEntries] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [ChatbotQueries]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatbotQueries]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ChatbotQueries] (
    [QueryId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Message] [nvarchar]  NOT NULL,
    [Response] [nvarchar]  NOT NULL,
    [CreatedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

SET IDENTITY_INSERT [ChatbotQueries] ON;
GO
INSERT INTO [ChatbotQueries] ([QueryId], [UserId], [Message], [Response], [CreatedAt], [ApplicationUserId]) VALUES (1, N'User1', N'How can I submit my custom cosplay gallery?', N'You can go to the Fan Submissions section from your user dashboard and upload your cosplay photos with a title and description.', N'09/24/2026 10:30:00', NULL);
INSERT INTO [ChatbotQueries] ([QueryId], [UserId], [Message], [Response], [CreatedAt], [ApplicationUserId]) VALUES (2, N'User2', N'Where can I find the upcoming anime event schedule?', N'Check out the Events page in the navigation bar to see all upcoming conventions, tournaments, and live streams.', N'09/24/2026 12:45:00', NULL);
INSERT INTO [ChatbotQueries] ([QueryId], [UserId], [Message], [Response], [CreatedAt], [ApplicationUserId]) VALUES (3, N'Guest', N'hi', N'Sorry, I don''t have an answer for that yet. Try browsing our FAQ or reach out through the Contact page.', N'09/27/2026 19:47:57', NULL);
SET IDENTITY_INSERT [ChatbotQueries] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [Contents]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Contents]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Contents] (
    [ContentId] [int] IDENTITY(1,1) NOT NULL,
    [CategoryId] [int]  NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [Type] [nvarchar]  NOT NULL,
    [Description] [nvarchar]  NULL,
    [ReleaseDate] [datetime2]  NOT NULL,
    [PopularityScore] [int]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [Contents] ON;
GO
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (1, 1, N'Solo Leveling Season 2 Teaser Breakdown', N'video', N'In-depth analysis of Sung Jinwoo''s Shadow Monarch powers and upcoming Monarch fights.', N'09/26/2026 16:32:28', 98);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (2, 1, N'One Piece Episode 1115 Stream & Discussion', N'video', N'The Straw Hats face off against the Marine Admirals in Egghead Island.', N'09/24/2026 16:32:28', 95);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (3, 2, N'Genshin Impact Natlan Exploration Guide', N'article', N'Complete guide to unlocking waypoints, chest puzzles, and Pyro Archon quests.', N'09/23/2026 16:32:28', 91);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (4, 5, N'Blackpink World Tour Highlight Reel', N'video', N'Behind-the-scenes footage and backstage moments from the Born Pink tour.', N'09/21/2026 16:32:28', 89);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (5, 2, N'Elden Ring Shadow of the Erdtree Full Walkthrough', N'article', N'Complete strategy guide for boss fights, weapons and secrets.', N'09/27/2026 17:21:27', 97);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (6, 3, N'Marvel Cinematic Universe Phase 6 Roadmap Unveiled', N'article', N'Breaking down upcoming Avengers movies, casting news and multiverse lore.', N'09/27/2026 17:21:27', 95);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (7, 4, N'Stranger Things Season 5 Final Battle Teaser', N'video', N'The Hawkins gang unites for the ultimate battle against Vecna.', N'09/27/2026 17:21:27', 94);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (8, 5, N'BTS World Reunion Tour Stadium Concert Announcement', N'video', N'HYBE reveals global tour dates across Asia, America and Europe.', N'09/27/2026 17:21:27', 99);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (9, 6, N'Batman Secret Files: Gotham City Multiverse Revealed', N'article', N'DC Comics announces milestone crossover event.', N'09/27/2026 17:21:27', 90);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (10, 7, N'Chainsaw Man Part 2 Manga Chapter Breakdown', N'article', N'Tatsuki Fujimoto delivers mind-bending twists in the latest arc.', N'09/27/2026 17:21:27', 93);
INSERT INTO [Contents] ([ContentId], [CategoryId], [Title], [Type], [Description], [ReleaseDate], [PopularityScore]) VALUES (11, 8, N'World Cosplay Summit 2026 Crafting & Armor Showcase', N'video', N'Top cosplayers demonstrate LED prop building and costume techniques.', N'09/27/2026 17:21:27', 92);
SET IDENTITY_INSERT [Contents] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [ContentTags]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentTags] (
    [ContentId] [int]  NOT NULL,
    [TagId] [int]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [ContentViews]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentViews]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentViews] (
    [ViewId] [int] IDENTITY(1,1) NOT NULL,
    [ContentId] [int]  NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ViewedAt] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [EventCategories]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EventCategories] (
    [EventId] [int]  NOT NULL,
    [CategoryId] [int]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [EventChatMessages]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventChatMessages]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EventChatMessages] (
    [MessageId] [int] IDENTITY(1,1) NOT NULL,
    [EventId] [int]  NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [UserName] [nvarchar]  NOT NULL,
    [Message] [nvarchar]  NOT NULL,
    [SentAt] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [Events]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Events]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Events] (
    [EventId] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [Description] [nvarchar]  NULL,
    [EventDate] [datetime2]  NOT NULL,
    [City] [nvarchar]  NULL,
    [LocationCoordinates] [nvarchar]  NULL
);
END
GO

SET IDENTITY_INSERT [Events] ON;
GO
INSERT INTO [Events] ([EventId], [Title], [Description], [EventDate], [City], [LocationCoordinates]) VALUES (1, N'Anime Expo 2026 International Convention', N'The largest anime, cosplay, and Japanese pop-culture convention bringing together voice actors, studios, and fans worldwide.', N'12/27/2026 16:32:28', N'Los Angeles', N'34.0407, -118.2698');
INSERT INTO [Events] ([EventId], [Title], [Description], [EventDate], [City], [LocationCoordinates]) VALUES (2, N'Comic-Con International 2026 San Diego', N'Premier comic book, movie, and pop culture convention featuring trailer drops, panel discussions, and celebrity signings.', N'01/27/2027 16:32:28', N'San Diego', N'32.7065, -117.1620');
INSERT INTO [Events] ([EventId], [Title], [Description], [EventDate], [City], [LocationCoordinates]) VALUES (3, N'Global Gaming Fest & Esports Championship', N'Worldwide gaming Expo featuring playable AAA demos, indie showcases, and high-stakes tournament finals.', N'02/27/2027 16:32:28', N'Tokyo', N'35.6762, 139.6503');
INSERT INTO [Events] ([EventId], [Title], [Description], [EventDate], [City], [LocationCoordinates]) VALUES (4, N'K-Pop World Tour Live In Seoul & Global Stream', N'Spectacular live concert experience bringing top K-Pop idols together for a 3-day arena stadium extravaganza.', N'03/27/2027 16:32:28', N'Seoul', N'37.5665, 126.9780');
SET IDENTITY_INSERT [Events] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [EventTicketLinks]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventTicketLinks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[EventTicketLinks] (
    [TicketLinkId] [int] IDENTITY(1,1) NOT NULL,
    [EventId] [int]  NOT NULL,
    [PlatformName] [nvarchar]  NOT NULL,
    [Url] [nvarchar]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [FanSubmissions]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FanSubmissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FanSubmissions] (
    [SubmissionId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [Content] [nvarchar]  NOT NULL,
    [MediaUrl] [nvarchar]  NULL,
    [Status] [nvarchar]  NOT NULL,
    [SubmittedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [FeaturedArticles]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeaturedArticles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[FeaturedArticles] (
    [ArticleId] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [Content] [nvarchar]  NOT NULL,
    [ImageUrl] [nvarchar]  NULL,
    [PublishedAt] [datetime2]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [FeaturedArticles] ON;
GO
INSERT INTO [FeaturedArticles] ([ArticleId], [Title], [Content], [ImageUrl], [PublishedAt]) VALUES (1, N'Solo Leveling Season 2: Arise from the Shadows Breakdown', N'Solo Leveling Season 2 explores Sung Jinwoo''s epic battles against the Monarchs, introducing brand new shadow army allies and high-octane animation sequences.', N'/images/content/content-hero-solo-leveling.png', N'09/25/2026 16:32:28');
INSERT INTO [FeaturedArticles] ([ArticleId], [Title], [Content], [ImageUrl], [PublishedAt]) VALUES (2, N'One Piece Egghead Island Arc Reaches Epic Conclusion', N'Eichiro Oda delivers masterful storytelling as Luffy and the Straw Hat Pirates uncover ancient secrets of the Void Century on Doctor Vegapunk''s island.', N'/images/explore/one-piece.png', N'09/22/2026 16:32:28');
INSERT INTO [FeaturedArticles] ([ArticleId], [Title], [Content], [ImageUrl], [PublishedAt]) VALUES (3, N'Demon Slayer Hashira Training & Infinity Castle Movie Announcement', N'ufotable announces the Infinity Castle Trilogy, bringing Tanjiro''s final battle against Muzan Kibutsuji to cinema screens worldwide.', N'/images/explore/demon-slayer.png', N'09/20/2026 16:32:28');
INSERT INTO [FeaturedArticles] ([ArticleId], [Title], [Content], [ImageUrl], [PublishedAt]) VALUES (4, N'Genshin Impact Version 5.0 Natlan Pyro Nation Unveiled', N'HoYoverse reveals the fiery nation of Natlan featuring dragon-riding mechanics, new playable Archons, and high-energy music tracks.', N'/images/explore/genshin-impact.png', N'09/17/2026 16:32:28');
SET IDENTITY_INSERT [FeaturedArticles] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [Feedbacks]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Feedbacks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Feedbacks] (
    [FeedbackId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Type] [nvarchar]  NOT NULL,
    [Message] [nvarchar]  NOT NULL,
    [Status] [nvarchar]  NOT NULL,
    [CreatedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [MediaItems]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MediaItems] (
    [MediaId] [int] IDENTITY(1,1) NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [FileUrl] [nvarchar]  NULL,
    [MediaType] [nvarchar]  NULL,
    [UploadedAt] [datetime2]  NOT NULL,
    [CategoryId] [int]  NULL
);
END
GO

SET IDENTITY_INSERT [MediaItems] ON;
GO
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (1, N'Solo Leveling Season 2 Trailer', N'/images/category_media/anime/videos/solo-leveling-s2-video-design.mp4', N'Video', N'09/27/2026 16:32:28', NULL);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (2, N'Anime Expo 2026 Cosplay Showcase', N'/images/category_media/anime/videos/solo-leveling-s2-video-design.mp4', N'Video', N'09/26/2026 16:32:28', NULL);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (3, N'FanHub+ Official Theme Music Track', N'/uploads/media/c9c8645161b94b57b2a61e23fc6d9d19.mp4', N'Audio', N'09/25/2026 16:32:28', NULL);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (4, N'Genshin Impact Version 5.0 Natlan Pyro Nation Gameplay Trailer', N'/images/category_media/gaming/videos/Michel-Loving-Husband.mp4', N'Trailer', N'09/27/2026 17:42:42', 2);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (5, N'Spider-Man Beyond the Spider-Verse Official Teaser', N'/images/category_media/comics/videos/this-video-not-x27-s-mine-c.mp4', N'Trailer', N'09/27/2026 17:42:42', 3);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (6, N'Stranger Things Season 5 Final Battle Official Trailer', N'/images/category_media/anime/audio/playtime.mp4', N'Trailer', N'09/27/2026 17:42:42', 4);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (7, N'Blackpink Born Pink World Tour Live Showcase', N'/images/category_media/anime/audio/playtime.mp4', N'Video', N'09/27/2026 17:42:42', 5);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (8, N'Batman Secret Files Multiverse Comic Teaser', N'/images/category_media/comics/videos/this-video-not-x27-s-mine-c.mp4', N'Video', N'09/27/2026 17:42:42', 6);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (9, N'One Piece Egghead Island Climax Episode Trailer', N'/images/category_media/anime/audio/playtime.mp4', N'Trailer', N'09/27/2026 17:42:42', 7);
INSERT INTO [MediaItems] ([MediaId], [Title], [FileUrl], [MediaType], [UploadedAt], [CategoryId]) VALUES (10, N'World Cosplay Summit Championship Final Highlights', N'/images/category_media/anime/audio/playtime.mp4', N'Video', N'09/27/2026 17:42:42', 8);
SET IDENTITY_INSERT [MediaItems] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [MediaRatings]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaRatings]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MediaRatings] (
    [RatingId] [int] IDENTITY(1,1) NOT NULL,
    [MediaId] [int]  NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Rating] [int]  NOT NULL,
    [CreatedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [MediaTags]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaTags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MediaTags] (
    [MediaId] [int]  NOT NULL,
    [TagId] [int]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [MerchandiseItems]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MerchandiseItems]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MerchandiseItems] (
    [ItemId] [int] IDENTITY(1,1) NOT NULL,
    [CategoryId] [int]  NOT NULL,
    [Name] [nvarchar]  NOT NULL,
    [ImageUrl] [nvarchar]  NULL,
    [Tag] [nvarchar]  NULL,
    [IsUpcoming] [bit]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [MerchandiseItems] ON;
GO
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (1, 1, N'Demon Slayer Rengoku Flame Oversized Hoodie', N'/images/merchandise/demon-slayer-hoodie.png', N'Limited Edition', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (2, 2, N'Genshin Impact Celestial Archon Desk Mat & Poster', N'/images/merchandise/genshin-poster.png', N'Popular', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (3, 1, N'Monkey D. Luffy Gear 5 Awakening Collectible Figure', N'/images/merchandise/luffy-figure.png', N'Collectible', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (4, 1, N'Sung Jinwoo Monarch of Shadows 1/6 Scale Statue', N'/images/merchandise/sung-jinwoo-figure.png', N'Pre-Order', NULL);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (5, 2, N'Elden Ring Malenia Blade of Miquella 1/4 Statue', N'/images/merchandise/genshin-poster.png', N'Collectible', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (6, 3, N'Iron Man Mark 85 Diecast Replica Helmet', N'/images/merchandise/demon-slayer-hoodie.png', N'Popular', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (7, 4, N'Stranger Things Hellfire Club Replica Denim Jacket', N'/images/merchandise/demon-slayer-hoodie.png', N'Limited Edition', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (8, 5, N'Blackpink Official Lightstick Ver 2 & Concert Album', N'/images/merchandise/genshin-poster.png', N'Popular', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (9, 6, N'Spider-Man Web-Slinger Limited Comic Artbook', N'/images/merchandise/luffy-figure.png', N'Pre-Order', NULL);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (10, 7, N'One Piece Wano Kuni Zoro Katana Replica Set', N'/images/merchandise/luffy-figure.png', N'Collectible', 0);
INSERT INTO [MerchandiseItems] ([ItemId], [CategoryId], [Name], [ImageUrl], [Tag], [IsUpcoming]) VALUES (11, 8, N'Cyberpunk Neon Wings LED Cosplay Armor Kit', N'/images/merchandise/sung-jinwoo-figure.png', N'Pre-Order', NULL);
SET IDENTITY_INSERT [MerchandiseItems] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [MerchandiseViews]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MerchandiseViews]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MerchandiseViews] (
    [ViewId] [int] IDENTITY(1,1) NOT NULL,
    [ItemId] [int]  NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ViewedAt] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [Notes]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Notes] (
    [NoteId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [Content] [nvarchar]  NOT NULL,
    [CreatedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [PollOptions]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PollOptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PollOptions] (
    [OptionId] [int] IDENTITY(1,1) NOT NULL,
    [PollId] [int]  NOT NULL,
    [OptionText] [nvarchar]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [PollOptions] ON;
GO
INSERT INTO [PollOptions] ([OptionId], [PollId], [OptionText]) VALUES (1, 1, N'Solo Leveling Season 2: Arise');
INSERT INTO [PollOptions] ([OptionId], [PollId], [OptionText]) VALUES (2, 1, N'One Piece: Egghead Climax');
INSERT INTO [PollOptions] ([OptionId], [PollId], [OptionText]) VALUES (3, 1, N'Demon Slayer: Infinity Castle');
INSERT INTO [PollOptions] ([OptionId], [PollId], [OptionText]) VALUES (4, 1, N'Genshin Impact: Natlan Pyro Nation');
SET IDENTITY_INSERT [PollOptions] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [Polls]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Polls]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Polls] (
    [PollId] [int] IDENTITY(1,1) NOT NULL,
    [CategoryId] [int]  NULL,
    [Title] [nvarchar]  NOT NULL,
    [IsActive] [bit]  NOT NULL,
    [CreatedAt] [datetime2]  NOT NULL
);
END
GO

SET IDENTITY_INSERT [Polls] ON;
GO
INSERT INTO [Polls] ([PollId], [CategoryId], [Title], [IsActive], [CreatedAt]) VALUES (1, NULL, N'Which Fandom Arc Are You Most Excited For In 2026?', NULL, N'09/27/2026 16:32:28');
SET IDENTITY_INSERT [Polls] OFF;
GO
GO

-- ---------------------------------------------------------
-- Table Structure for [PollVotes]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PollVotes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PollVotes] (
    [VoteId] [int] IDENTITY(1,1) NOT NULL,
    [PollId] [int]  NOT NULL,
    [OptionId] [int]  NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [VotedAt] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [QuizAttempts]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuizAttempts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[QuizAttempts] (
    [AttemptId] [int] IDENTITY(1,1) NOT NULL,
    [QuizId] [int]  NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Score] [int]  NOT NULL,
    [TotalQuestions] [int]  NOT NULL,
    [PercentageScore] [float]  NOT NULL,
    [AttemptedAt] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [QuizQuestions]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuizQuestions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[QuizQuestions] (
    [QuestionId] [int] IDENTITY(1,1) NOT NULL,
    [QuizId] [int]  NOT NULL,
    [QuestionText] [nvarchar]  NOT NULL,
    [OptionA] [nvarchar]  NOT NULL,
    [OptionB] [nvarchar]  NOT NULL,
    [OptionC] [nvarchar]  NOT NULL,
    [OptionD] [nvarchar]  NOT NULL,
    [CorrectOption] [nvarchar]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [Quizzes]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Quizzes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Quizzes] (
    [QuizId] [int] IDENTITY(1,1) NOT NULL,
    [CategoryId] [int]  NOT NULL,
    [Title] [nvarchar]  NOT NULL,
    [Description] [nvarchar]  NULL,
    [CreatedAt] [datetime2]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [Shares]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Shares]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Shares] (
    [ShareId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [Platform] [nvarchar]  NOT NULL,
    [SharedUrl] [nvarchar]  NOT NULL,
    [SharedAt] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [Tags]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tags]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tags] (
    [TagId] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar]  NOT NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [UserActivities]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserActivities]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserActivities] (
    [ActivityId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ActionDescription] [nvarchar]  NOT NULL,
    [Timestamp] [datetime2]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [UserFavoriteCategories]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserFavoriteCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserFavoriteCategories] (
    [FavoriteId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [CategoryId] [int]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

-- ---------------------------------------------------------
-- Table Structure for [UserPreferences]
-- ---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPreferences]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserPreferences] (
    [PreferenceId] [int] IDENTITY(1,1) NOT NULL,
    [UserId] [nvarchar]  NOT NULL,
    [ThemeMode] [nvarchar]  NOT NULL,
    [FontSize] [nvarchar]  NOT NULL,
    [EmailNotifications] [bit]  NOT NULL,
    [ApplicationUserId] [nvarchar]  NULL
);
END
GO

