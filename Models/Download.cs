// Download.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;



namespace musicApp.Models;

[Table("downloads")]
public class Download 
{
    [Key]
    [Column("download_id")]
    public int DownloadId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("song_id")]
    public int SongId { get; set; }
    
    [Column("downloaded_at")]
    public DateTime DownloadedAt { get; set; }
}