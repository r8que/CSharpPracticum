-- Показ всех треков длинною более 200 секунд
SELECT Title, DurationSeconds, IsExplicit
FROM Tracks
WHERE DurationSeconds > 200
ORDER BY DurationSeconds DESC;

-- Присвоение пользователю премиум статуса
UPDATE Users
SET IsPremium = 1
WHERE Username = N'katya_listener';

-- Удаление трека из плейлиста
DELETE FROM PlaylistTracks
WHERE TrackId = (SELECT TrackId FROM Tracks WHERE Title = N'Thunder');

-- Количество треков в каждом альбоме
SELECT a.Title AS Album, COUNT(t.TrackId) AS TracksCount
FROM Albums a
LEFT JOIN Tracks t ON a.AlbumId = t.AlbumId
GROUP BY a.Title
ORDER BY TracksCount DESC;