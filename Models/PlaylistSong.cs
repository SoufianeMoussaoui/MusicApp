namespace musicApp.Models;
// PlaylistSong.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Supabase.Postgrest.Models;

[Table("playlist_songs")]
public class PlaylistSong : BaseModel
{
    [Column("playlist_song_id")]
    public int PlaylistSongId { get; set; }
    
    [Column("playlist_id")]
    public int PlaylistId { get; set; }
    
    [Column("song_id")]
    public int SongId { get; set; } 
    
    [Column("order_position")]
    public int OrderPosition { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}