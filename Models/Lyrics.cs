using System.ComponentModel.DataAnnotations;

namespace musicApp.Models;

public class Lyrics
{
    public int LyricsId { get; set; }
    public int SongId { get; set; }
    public string? LyricsText { get; set; }
    public string? LyricsSource { get; set; }
    [DataType(DataType.Date)]
    public DateTime AddedAt { get; set; }
}
