using System.Collections.Generic;

namespace musicApp.Models;

public class ProfileViewModel
{
    public User user { get; set; }
    public List<Song> RecentlyPlayed { get; set; } = new List<Song>();
    public List<Playlist> Playlists { get; set; } = new List<Playlist>();
    public List<Download> Downloads { get; set; } = new List<Download>();
    public int TotalPlays { get; set; }
    public int TotalPlaylists { get; set; }
    public int TotalDownloads { get; set; }
    public bool IsAuthenticated { get; set; }
}
