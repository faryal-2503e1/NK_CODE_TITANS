using Microsoft.EntityFrameworkCore;
using FanHubPlus.Models;

namespace FanHubPlus.Data
{
    public static class SeedData
    {
        private static readonly (string Name, string Description)[] DefaultCategories =
        {
            ("Anime", "Anime series, films and the characters behind them."),
            ("Gaming", "Video games, esports and gaming culture."),
            ("Movies", "Feature films, premieres and trailers."),
            ("TV Shows", "Television series, seasons and episode news."),
            ("K-Pop", "Korean pop artists, releases and fan culture."),
            ("Comics", "Comic books, graphic novels and creators."),
            ("Manga", "Manga series, chapters and volume releases."),
            ("Cosplay", "Cosplay showcases, meetups and craft guides.")
        };

        public static async Task SeedAllDataAsync(FanHubContext context)
        {
            await EnsureCategoriesAsync(context);
            await SeedChatbotFaqsAsync(context);
            await SeedCharactersAsync(context);
            await SeedArticlesAsync(context);
            await SeedEventsAsync(context);
            await SeedMerchandiseAsync(context);
            await SeedContentsAsync(context);
            await SeedMediaItemsAsync(context);
            await SeedPollsAsync(context);
        }

        public static async Task EnsureCategoriesAsync(FanHubContext context)
        {
            var existing = await context.Categories.Select(c => c.Name.ToLower()).ToListAsync();

            foreach (var (name, description) in DefaultCategories)
            {
                if (!existing.Contains(name.ToLower()))
                {
                    context.Categories.Add(new Category { Name = name, Description = description });
                }
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedChatbotFaqsAsync(FanHubContext context)
        {
            if (await context.ChatbotFaqs.AnyAsync()) return;

            var faqs = new List<ChatbotFaq>
            {
                new ChatbotFaq
                {
                    Question = "what is this website",
                    Answer = "FanHubPlus is a community hub for fans to explore content, characters, events, and merchandise, and connect with other fans who share the same interests."
                },
                new ChatbotFaq
                {
                    Question = "how do i bookmark something",
                    Answer = "Open any content, character, media or merchandise page and click the Bookmark button. You need to be logged in to save bookmarks."
                },
                new ChatbotFaq
                {
                    Question = "how do i submit fan art",
                    Answer = "Go to Fan Submissions from your dashboard and click \"Submit Your Work\". Every submission is reviewed before it's published."
                },
                new ChatbotFaq
                {
                    Question = "how do i create an account",
                    Answer = "Click Register at the top of the page and fill in your email and password to get started."
                },
                new ChatbotFaq
                {
                    Question = "how do i contact support",
                    Answer = "You can reach out through the Contact page, or send us feedback from your dashboard's Feedback section."
                },
                new ChatbotFaq
                {
                    Question = "how do i change my profile picture",
                    Answer = "Go to your Profile, click Edit Profile, and upload a new picture from the Profile Picture section."
                }
            };

            context.ChatbotFaqs.AddRange(faqs);
            await context.SaveChangesAsync();
        }

        public static async Task SeedCharactersAsync(FanHubContext context)
        {
            if (await context.CharacterProfiles.AnyAsync()) return;

            var animeCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Anime");
            var comicsCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Comics");
            int animeId = animeCategory?.CategoryId ?? 1;
            int comicsId = comicsCategory?.CategoryId ?? 6;

            var characters = new List<CharacterProfile>
            {
                new CharacterProfile
                {
                    Name = "Sung Jinwoo",
                    CategoryId = animeId,
                    Bio = "The Shadow Monarch and the weakest E-rank hunter who became humanity's strongest S-rank hunter.",
                    ImageUrl = "/images/content/characters/sung-jinwoo.png"
                },
                new CharacterProfile
                {
                    Name = "Cha Hae-In",
                    CategoryId = animeId,
                    Bio = "S-Rank Hunter and Vice-Chairman of the Hunters Guild. Known as the Master Swordmaster.",
                    ImageUrl = "/images/content/characters/cha-haein.png"
                },
                new CharacterProfile
                {
                    Name = "Go Gunhee",
                    CategoryId = animeId,
                    Bio = "Chairman of the Korean Hunters Association. A powerful S-rank hunter dedicated to defending humanity.",
                    ImageUrl = "/images/content/characters/go-gunhee.png"
                },
                new CharacterProfile
                {
                    Name = "Beru",
                    CategoryId = animeId,
                    Bio = "The Ant King turned Shadow Marshal, loyal protector of Sung Jinwoo and ruler of shadow armies.",
                    ImageUrl = "/images/content/characters/beru.png"
                },
                new CharacterProfile
                {
                    Name = "Monkey D. Luffy",
                    CategoryId = animeId,
                    Bio = "Captain of the Straw Hat Pirates, seeking the One Piece to become the King of the Pirates.",
                    ImageUrl = "/images/explore/one-piece.png"
                },
                new CharacterProfile
                {
                    Name = "Tanjiro Kamado",
                    CategoryId = animeId,
                    Bio = "Demon Slayer of the Demon Slayer Corps wielding Sun Breathing to save his sister Nezuko.",
                    ImageUrl = "/images/explore/demon-slayer.png"
                },
                new CharacterProfile
                {
                    Name = "Satoru Gojo",
                    CategoryId = animeId,
                    Bio = "The strongest Jujutsu Sorcerer, master of Limitless and Six Eyes.",
                    ImageUrl = "/images/explore/jujutsu-kaisen.png"
                },
                new CharacterProfile
                {
                    Name = "Peter Parker (Spider-Man)",
                    CategoryId = comicsId,
                    Bio = "Your friendly neighborhood Spider-Man protecting New York City with spider powers.",
                    ImageUrl = "/images/explore/spider-man.png"
                }
            };

            context.CharacterProfiles.AddRange(characters);
            await context.SaveChangesAsync();
        }

        public static async Task SeedArticlesAsync(FanHubContext context)
        {
            if (await context.FeaturedArticles.AnyAsync()) return;

            var articles = new List<FeaturedArticle>
            {
                new FeaturedArticle
                {
                    Title = "Solo Leveling Season 2: Arise from the Shadows Breakdown",
                    Content = "Solo Leveling Season 2 explores Sung Jinwoo's epic battles against the Monarchs, introducing brand new shadow army allies and high-octane animation sequences.",
                    ImageUrl = "/images/content/content-hero-solo-leveling.png",
                    PublishedAt = DateTime.Now.AddDays(-2)
                },
                new FeaturedArticle
                {
                    Title = "One Piece Egghead Island Arc Reaches Epic Conclusion",
                    Content = "Eichiro Oda delivers masterful storytelling as Luffy and the Straw Hat Pirates uncover ancient secrets of the Void Century on Doctor Vegapunk's island.",
                    ImageUrl = "/images/explore/one-piece.png",
                    PublishedAt = DateTime.Now.AddDays(-5)
                },
                new FeaturedArticle
                {
                    Title = "Demon Slayer Hashira Training & Infinity Castle Movie Announcement",
                    Content = "ufotable announces the Infinity Castle Trilogy, bringing Tanjiro's final battle against Muzan Kibutsuji to cinema screens worldwide.",
                    ImageUrl = "/images/explore/demon-slayer.png",
                    PublishedAt = DateTime.Now.AddDays(-7)
                },
                new FeaturedArticle
                {
                    Title = "Genshin Impact Version 5.0 Natlan Pyro Nation Unveiled",
                    Content = "HoYoverse reveals the fiery nation of Natlan featuring dragon-riding mechanics, new playable Archons, and high-energy music tracks.",
                    ImageUrl = "/images/explore/genshin-impact.png",
                    PublishedAt = DateTime.Now.AddDays(-10)
                }
            };

            context.FeaturedArticles.AddRange(articles);
            await context.SaveChangesAsync();
        }

        public static async Task SeedEventsAsync(FanHubContext context)
        {
            if (await context.Events.AnyAsync()) return;

            var events = new List<Event>
            {
                new Event
                {
                    Title = "Anime Expo 2026 International Convention",
                    Description = "The largest anime, cosplay, and Japanese pop-culture convention bringing together voice actors, studios, and fans worldwide.",
                    City = "Los Angeles",
                    EventDate = DateTime.Now.AddMonths(3),
                    LocationCoordinates = "34.0407, -118.2698"
                },
                new Event
                {
                    Title = "Comic-Con International 2026 San Diego",
                    Description = "Premier comic book, movie, and pop culture convention featuring trailer drops, panel discussions, and celebrity signings.",
                    City = "San Diego",
                    EventDate = DateTime.Now.AddMonths(4),
                    LocationCoordinates = "32.7065, -117.1620"
                },
                new Event
                {
                    Title = "Global Gaming Fest & Esports Championship",
                    Description = "Worldwide gaming Expo featuring playable AAA demos, indie showcases, and high-stakes tournament finals.",
                    City = "Tokyo",
                    EventDate = DateTime.Now.AddMonths(5),
                    LocationCoordinates = "35.6762, 139.6503"
                },
                new Event
                {
                    Title = "K-Pop World Tour Live In Seoul & Global Stream",
                    Description = "Spectacular live concert experience bringing top K-Pop idols together for a 3-day arena stadium extravaganza.",
                    City = "Seoul",
                    EventDate = DateTime.Now.AddMonths(6),
                    LocationCoordinates = "37.5665, 126.9780"
                }
            };

            context.Events.AddRange(events);
            await context.SaveChangesAsync();
        }

        public static async Task SeedMerchandiseAsync(FanHubContext context)
        {
            if (await context.MerchandiseItems.AnyAsync()) return;

            var animeCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Anime");
            var gamingCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Gaming");

            var items = new List<MerchandiseItem>
            {
                new MerchandiseItem
                {
                    Name = "Demon Slayer Rengoku Flame Oversized Hoodie",
                    CategoryId = animeCategory?.CategoryId ?? 1,
                    ImageUrl = "/images/merchandise/demon-slayer-hoodie.png",
                    Tag = "Limited Edition",
                    IsUpcoming = false
                },
                new MerchandiseItem
                {
                    Name = "Genshin Impact Celestial Archon Desk Mat & Poster",
                    CategoryId = gamingCategory?.CategoryId ?? 2,
                    ImageUrl = "/images/merchandise/genshin-poster.png",
                    Tag = "Popular",
                    IsUpcoming = false
                },
                new MerchandiseItem
                {
                    Name = "Monkey D. Luffy Gear 5 Awakening Collectible Figure",
                    CategoryId = animeCategory?.CategoryId ?? 1,
                    ImageUrl = "/images/merchandise/luffy-figure.png",
                    Tag = "Collectible",
                    IsUpcoming = false
                },
                new MerchandiseItem
                {
                    Name = "Sung Jinwoo Monarch of Shadows 1/6 Scale Statue",
                    CategoryId = animeCategory?.CategoryId ?? 1,
                    ImageUrl = "/images/merchandise/sung-jinwoo-figure.png",
                    Tag = "Pre-Order",
                    IsUpcoming = true
                }
            };

            context.MerchandiseItems.AddRange(items);
            await context.SaveChangesAsync();
        }

        public static async Task SeedContentsAsync(FanHubContext context)
        {
            if (await context.Contents.AnyAsync()) return;

            var animeCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Anime");
            var gamingCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Gaming");
            var kpopCategory = await context.Categories.FirstOrDefaultAsync(c => c.Name == "K-Pop");

            var contents = new List<Content>
            {
                new Content
                {
                    Title = "Solo Leveling Season 2 Teaser Breakdown",
                    Type = "video",
                    CategoryId = animeCategory?.CategoryId ?? 1,
                    Description = "In-depth analysis of Sung Jinwoo's Shadow Monarch powers and upcoming Monarch fights.",
                    ReleaseDate = DateTime.Now.AddDays(-1),
                    PopularityScore = 98
                },
                new Content
                {
                    Title = "One Piece Episode 1115 Stream & Discussion",
                    Type = "video",
                    CategoryId = animeCategory?.CategoryId ?? 1,
                    Description = "The Straw Hats face off against the Marine Admirals in Egghead Island.",
                    ReleaseDate = DateTime.Now.AddDays(-3),
                    PopularityScore = 95
                },
                new Content
                {
                    Title = "Genshin Impact Natlan Exploration Guide",
                    Type = "article",
                    CategoryId = gamingCategory?.CategoryId ?? 2,
                    Description = "Complete guide to unlocking waypoints, chest puzzles, and Pyro Archon quests.",
                    ReleaseDate = DateTime.Now.AddDays(-4),
                    PopularityScore = 91
                },
                new Content
                {
                    Title = "Blackpink World Tour Highlight Reel",
                    Type = "video",
                    CategoryId = kpopCategory?.CategoryId ?? 5,
                    Description = "Behind-the-scenes footage and backstage moments from the Born Pink tour.",
                    ReleaseDate = DateTime.Now.AddDays(-6),
                    PopularityScore = 89
                }
            };

            context.Contents.AddRange(contents);
            await context.SaveChangesAsync();
        }

        public static async Task SeedMediaItemsAsync(FanHubContext context)
        {
            if (await context.MediaItems.AnyAsync()) return;

            var media = new List<MediaItem>
            {
                new MediaItem
                {
                    Title = "Solo Leveling Season 2 Trailer",
                    MediaType = "Video",
                    FileUrl = "/uploads/media/18544a4862554f36b0b4ec51115014c7.mp4",
                    UploadedAt = DateTime.Now
                },
                new MediaItem
                {
                    Title = "Anime Expo 2026 Cosplay Showcase",
                    MediaType = "Video",
                    FileUrl = "/uploads/media/5dcf424d0e78413496fd4b38e587b523.mp4",
                    UploadedAt = DateTime.Now.AddDays(-1)
                },
                new MediaItem
                {
                    Title = "FanHub+ Official Theme Music Track",
                    MediaType = "Audio",
                    FileUrl = "/uploads/media/c9c8645161b94b57b2a61e23fc6d9d19.mp4",
                    UploadedAt = DateTime.Now.AddDays(-2)
                }
            };

            context.MediaItems.AddRange(media);
            await context.SaveChangesAsync();
        }

        public static async Task SeedPollsAsync(FanHubContext context)
        {
            if (await context.Polls.AnyAsync()) return;

            var poll = new Poll
            {
                Title = "Which Fandom Arc Are You Most Excited For In 2026?",
                IsActive = true,
                CreatedAt = DateTime.Now,
                Options = new List<PollOption>
                {
                    new PollOption { OptionText = "Solo Leveling Season 2: Arise" },
                    new PollOption { OptionText = "One Piece: Egghead Climax" },
                    new PollOption { OptionText = "Demon Slayer: Infinity Castle" },
                    new PollOption { OptionText = "Genshin Impact: Natlan Pyro Nation" }
                }
            };

            context.Polls.Add(poll);
            await context.SaveChangesAsync();
        }
    }
}

