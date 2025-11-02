
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Postgrest.Models;
using Supabase;
namespace musicApp.Models;

[Table("songs")]
public class Song : BaseModel
{

    public int SongId { set; get; }
    public int UserId { get; set; }

    [Required(ErrorMessage = "Title required")]
    [NotNull]
    public string? Title { get; set; }
    [Required(ErrorMessage = "Title required")]
    [NotNull]
    public string? Artist { get; set; }

    public string? Album { get; set; }
    [NotNull]
    public int DurationSeconds { get; set; }
    public string? FilePath { get; set; }
    public string? CoverUrl {get; set;}
    public bool IsUserUploaded { get; set; }

}

/*

 
*/