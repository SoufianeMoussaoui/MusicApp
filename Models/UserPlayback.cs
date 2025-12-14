using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;



namespace musicApp.Models;


[Table("user_playback")]
public class UserPlayback 
{
    [Key]
    [Column("playback_id")]
    public int UserPlaybackId { get; set; }
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("song_id")]
    public int SongId { get; set; }
    
    //[Column("current_position")]
    //public List<Song> SongHistory { get; set; } = new List<Song>();

    [DataType(DataType.Date)]
    [Column("last_played")]
    public DateTime LastPlayed { get; set; }

}