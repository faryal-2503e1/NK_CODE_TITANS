USE [FanHub21];
GO

-- Update all MediaItems of type Video/Trailer to point to local reliable MP4 files
UPDATE MediaItems
SET FileUrl = '/images/category_media/anime/videos/solo-leveling-s2-video-design.mp4'
WHERE MediaType IN ('Video', 'Trailer') AND (CategoryId = 1 OR CategoryId IS NULL);

UPDATE MediaItems
SET FileUrl = '/images/category_media/gaming/videos/Michel-Loving-Husband.mp4'
WHERE MediaType IN ('Video', 'Trailer') AND CategoryId = 2;

UPDATE MediaItems
SET FileUrl = '/images/category_media/comics/videos/this-video-not-x27-s-mine-c.mp4'
WHERE MediaType IN ('Video', 'Trailer') AND CategoryId IN (3, 6);

UPDATE MediaItems
SET FileUrl = '/images/category_media/anime/audio/playtime.mp4'
WHERE MediaType IN ('Video', 'Trailer') AND CategoryId NOT IN (1, 2, 3, 6);

GO
