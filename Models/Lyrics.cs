using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace musicApp.Models;


[Table("lyrics")]
public class Lyrics 
{
    [Key]
    [Column("lyrics_id")]
    public int LyricsId { get; set; }
    
    [Column("song_id")]
    public int SongId { get; set; }
    
    [Column("lyrics_text")]
    public string? LyricsText { get; set; }
    
    [Column("lyrics_source")]
    public string? LyricsSource { get; set; }
    
    [Column("added_at")]
    public DateTime AddedAt { get; set; }
}