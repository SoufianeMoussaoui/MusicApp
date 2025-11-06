using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

using Supabase.Postgrest.Models;

namespace musicApp.Models;




[Table("albums")]
public class Album : BaseModel
{
    [Column("album_id")]
    public int AlbumId { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    [Column("title")]
    public string? Title { get; set; }
    
    [Required(ErrorMessage = "Artist is required")]
    [Column("artist")]
    public string? Artist { get; set; }
    
    [Required(ErrorMessage = "The album release year is required")]
    [Column("release_year")]
    public int ReleaseYear { get; set; }
    
    [Column("cover_image")]
    public string? CoverImage { get; set; }
    
    [Column("song_count")]
    public int SongCount { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}