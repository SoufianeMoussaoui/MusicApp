namespace musicApp.Models;

public class PlaylistSong
{
    public int PlaylistSongId { get; set; }
    public int PlaylistId { get; set; }
    public int SongId { get;  set; } 
    public int OrderPosition { get; set; }
}