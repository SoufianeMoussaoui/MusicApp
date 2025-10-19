using System.ComponentModel.DataAnnotations;
namespace musicApp.Models;


public class Playlist
{
    public int PlaylistId { get; set; }
    public int UserId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
}