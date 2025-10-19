using System.ComponentModel.DataAnnotations;

namespace musicApp.Models;

public class Artist : User
{
    public int ArtistId;
    public List<Uri> Links { get; set; } = new List<Uri>();
    public string? Bio;
}