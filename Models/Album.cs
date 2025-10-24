using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace musicApp.Models;


public class Album
{
    public int AlbumId { get; set; }
    [NotNull]
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    [NotNull]
    [Required(ErrorMessage = "Name is required")]
    public string? Artist { get; set; }
    [NotNull]
    [Required(ErrorMessage ="The albmu release year is required")]
    public int ReleaseYear { get; set; }
    [Required]
    public string? CoverImage { get; set; }
}