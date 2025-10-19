namespace musicApp.Models;


public class Album
{
    public int AlbumId { get; set; }
    public string? Name { get; set; }
    public string? Artist { get; set; }
    public int ReleaseYear { get; set; }
    public string? CoverImage { get; set; }
}