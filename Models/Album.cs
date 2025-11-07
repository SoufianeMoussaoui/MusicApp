using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
namespace musicApp.Models;

public class Album
{
    [Key]
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
    
    [Column("song_id")]
    public int SongId { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    
}