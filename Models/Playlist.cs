// Playlist.cs
using System.ComponentModel.DataAnnotations;
using Supabase;

using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Supabase.Postgrest.Models;


namespace musicApp.Models;

[Table("playlists")]
public class Playlist : BaseModel
{
    [Column("playlist_id")]
    public int PlaylistId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("name")]
    public string? Name { get; set; }
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}