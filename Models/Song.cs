
using System.ComponentModel.DataAnnotations.Schema;

namespace musicApp.Models;

public class Song
{

    public int SongId { set; get; }
    public int UserId { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Album { get; set; }
    public int DurationSeconds { get; set; }
    public string? FilePath { get; set; }
    public bool IsUserUploaded { get; set; }

}

/*

 
*/