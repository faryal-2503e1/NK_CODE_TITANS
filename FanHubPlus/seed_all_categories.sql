USE [FanHub21];
GO

-- 1. Ensure all 8 categories exist
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 1) INSERT INTO Categories (Name, Description) VALUES (N'Anime', N'Anime series, films and the characters behind them.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 2) INSERT INTO Categories (Name, Description) VALUES (N'Gaming', N'Video games, esports and gaming culture.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 3) INSERT INTO Categories (Name, Description) VALUES (N'Movies', N'Feature films, premieres and trailers.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 4) INSERT INTO Categories (Name, Description) VALUES (N'TV Shows', N'Television series, seasons and episode news.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 5) INSERT INTO Categories (Name, Description) VALUES (N'K-Pop', N'Korean pop artists, releases and fan culture.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 6) INSERT INTO Categories (Name, Description) VALUES (N'Comics', N'Comic books, graphic novels and creators.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 7) INSERT INTO Categories (Name, Description) VALUES (N'Manga', N'Manga series, chapters and volume releases.');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryId = 8) INSERT INTO Categories (Name, Description) VALUES (N'Cosplay', N'Cosplay showcases, meetups and craft guides.');
GO

-- 2. Seed Characters for all categories
INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 2, N'Kratos (God of War)', N'The Ghost of Sparta who defeated the Olympian and Norse pantheons.', N'/images/explore/genshin-impact.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Kratos (God of War)');

INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 3, N'Tony Stark (Iron Man)', N'Genius, billionaire, playboy, philanthropist who saved the universe.', N'/images/explore/spider-man.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Tony Stark (Iron Man)');

INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 4, N'Eleven (Stranger Things)', N'A young girl with powerful psychokinetic abilities protecting Hawkins.', N'/images/explore/stranger-things.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Eleven (Stranger Things)');

INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 5, N'Jennie Kim (Blackpink)', N'Global icon, lead vocalist and rapper of world-renowned K-Pop group Blackpink.', N'/images/explore/blackpink.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Jennie Kim (Blackpink)');

INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 6, N'Batman (Bruce Wayne)', N'The Dark Knight of Gotham City fighting crime with martial arts and technology.', N'/images/explore/spider-man.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Batman (Bruce Wayne)');

INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 7, N'Zoro Roronoa', N'The Master Swordsman of the Straw Hat Pirates wielding three katanas.', N'/images/explore/one-piece.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Zoro Roronoa');

INSERT INTO CharacterProfiles (CategoryId, Name, Bio, ImageUrl)
SELECT 8, N'Cyber Neon Cosplay Queen', N'Renowned international cosplayer known for glowing armor and wings.', N'/assets/images/hero/cosplay-hero.png'
WHERE NOT EXISTS (SELECT 1 FROM CharacterProfiles WHERE Name = N'Cyber Neon Cosplay Queen');

-- 3. Seed Contents for all categories
INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 2, N'Elden Ring Shadow of the Erdtree Full Walkthrough', N'article', N'Complete strategy guide for boss fights, weapons and secrets.', GETDATE(), 97
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%Elden Ring%');

INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 3, N'Marvel Cinematic Universe Phase 6 Roadmap Unveiled', N'article', N'Breaking down upcoming Avengers movies, casting news and multiverse lore.', GETDATE(), 95
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%Phase 6%');

INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 4, N'Stranger Things Season 5 Final Battle Teaser', N'video', N'The Hawkins gang unites for the ultimate battle against Vecna.', GETDATE(), 94
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%Stranger Things%');

INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 5, N'BTS World Reunion Tour Stadium Concert Announcement', N'video', N'HYBE reveals global tour dates across Asia, America and Europe.', GETDATE(), 99
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%BTS World%');

INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 6, N'Batman Secret Files: Gotham City Multiverse Revealed', N'article', N'DC Comics announces milestone crossover event.', GETDATE(), 90
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%Batman Secret%');

INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 7, N'Chainsaw Man Part 2 Manga Chapter Breakdown', N'article', N'Tatsuki Fujimoto delivers mind-bending twists in the latest arc.', GETDATE(), 93
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%Chainsaw Man%');

INSERT INTO Contents (CategoryId, Title, Type, Description, ReleaseDate, PopularityScore)
SELECT 8, N'World Cosplay Summit 2026 Crafting & Armor Showcase', N'video', N'Top cosplayers demonstrate LED prop building and costume techniques.', GETDATE(), 92
WHERE NOT EXISTS (SELECT 1 FROM Contents WHERE Title LIKE N'%World Cosplay%');

-- 4. Seed Merchandise for all categories
INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 2, N'Elden Ring Malenia Blade of Miquella 1/4 Statue', N'/images/merchandise/genshin-poster.png', N'Collectible', 0
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Malenia%');

INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 3, N'Iron Man Mark 85 Diecast Replica Helmet', N'/images/merchandise/demon-slayer-hoodie.png', N'Popular', 0
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Iron Man%');

INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 4, N'Stranger Things Hellfire Club Replica Denim Jacket', N'/images/merchandise/demon-slayer-hoodie.png', N'Limited Edition', 0
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Hellfire%');

INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 5, N'Blackpink Official Lightstick Ver 2 & Concert Album', N'/images/merchandise/genshin-poster.png', N'Popular', 0
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Lightstick%');

INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 6, N'Spider-Man Web-Slinger Limited Comic Artbook', N'/images/merchandise/luffy-figure.png', N'Pre-Order', 1
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Web-Slinger%');

INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 7, N'One Piece Wano Kuni Zoro Katana Replica Set', N'/images/merchandise/luffy-figure.png', N'Collectible', 0
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Wano%');

INSERT INTO MerchandiseItems (CategoryId, Name, ImageUrl, Tag, IsUpcoming)
SELECT 8, N'Cyberpunk Neon Wings LED Cosplay Armor Kit', N'/images/merchandise/sung-jinwoo-figure.png', N'Pre-Order', 1
WHERE NOT EXISTS (SELECT 1 FROM MerchandiseItems WHERE Name LIKE N'%Cyberpunk Neon%');

GO
