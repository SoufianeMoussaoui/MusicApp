using System.Collections.Generic;

namespace musicApp.Models;

public class ProfileViewModel
{
    public User user { get; set; }
    public List<SongWithPlaybackInfo>? RecentlyPlayed { get; set; }  // Changed to RecentlyPlayedSong
    public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    public List<Download> Downloads { get; set; } = new List<Download>();
    public int TotalPlays { get; set; }
    public int TotalPlaylists { get; set; }
    public int TotalDownloads { get; set; }
    public string TotalTimePlayedFormatted { get; set; }
    public int TotalHoursPlayed { get; set; }
    public int TotalMinutesPlayed { get; set; }

}
    public class SongWithPlaybackInfo
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string? CoverPath { get; set; }
        public int PlayCounts { get; set; }
        public int DurationSeconds { get; set; }
        public DateTime LastPlayed { get; set; }
    }