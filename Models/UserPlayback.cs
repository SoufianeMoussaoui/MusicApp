using System.ComponentModel.DataAnnotations;

namespace musicApp.Models;


public class UserPlayback
{
    public int UserPlaybackId { get; set; }
    public int UserId { get; set; }
    public int SongId { get; set; }
    public int CurrentPosition { get; set; }
    [DataType(DataType.Date)]
    public DateTime LastPlayed { get; set; }

}