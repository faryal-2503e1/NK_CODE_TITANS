USE [FanHub21];
GO

-- Populate MediaItems for Trailers / Videos across Categories
INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'Solo Leveling Season 2: Arise Official Trailer', N'https://www.youtube.com/embed/a6n38g9Rrn0', N'Trailer', 1, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Solo Leveling%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'Genshin Impact Version 5.0 Natlan Pyro Nation Gameplay Trailer', N'https://www.youtube.com/embed/1JjO8Yk3fT8', N'Trailer', 2, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Natlan%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'Spider-Man Beyond the Spider-Verse Official Teaser', N'https://www.youtube.com/embed/shW9i6k8cB0', N'Trailer', 3, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Spider-Verse%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'Stranger Things Season 5 Final Battle Official Trailer', N'https://www.youtube.com/embed/b9EkMc79ZSU', N'Trailer', 4, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Stranger Things%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'Blackpink Born Pink World Tour Live Showcase', N'https://www.youtube.com/embed/gQlMMD8auMs', N'Video', 5, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Born Pink%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'Batman Secret Files Multiverse Comic Teaser', N'https://www.youtube.com/embed/shW9i6k8cB0', N'Video', 6, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Batman Secret%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'One Piece Egghead Island Climax Episode Trailer', N'https://www.youtube.com/embed/q0_fIEnptXg', N'Trailer', 7, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Egghead%');

INSERT INTO MediaItems (Title, FileUrl, MediaType, CategoryId, UploadedAt)
SELECT N'World Cosplay Summit Championship Final Highlights', N'/images/category_media/anime/audio/playtime.mp4', N'Video', 8, GETDATE()
WHERE NOT EXISTS (SELECT 1 FROM MediaItems WHERE Title LIKE N'%Summit%');

GO
