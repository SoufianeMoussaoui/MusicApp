using System.ComponentModel.DataAnnotations.Schema;
using Supabase;
using Supabase.Postgrest.Models;


namespace musicApp.Models;


[Table("lyrics")]
public class Lyrics : BaseModel
{
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