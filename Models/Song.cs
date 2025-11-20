
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;


namespace musicApp.Models;

public class Song 
{
    [Key]
    [Column("song_id")]
    public int SongId { set; get; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Title required")]
    [NotNull]
    public string? Title { get; set; }
    
    [Column("artist_id")]
    public int? ArtistId { get; set; }

    public int AlbumId { get; set; }

    [NotNull]
    public int DurationSeconds { get; set; }
    public int PlayCounts {get; set;} = 0;
    
    public string? FilePath { get; set; }
    
    public string? CoverPath {get; set;}

    public DateTime UploadeAt {get; set;}
    public bool IsUserUploaded { get; set; } = false;

}

