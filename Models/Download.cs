using System.ComponentModel.DataAnnotations;
namespace musicApp.Models;

public class Download
{
    public int DownloadId { get; set; }
    public int UserId { get; set; }
    public int SongId { get; set; }
    [DataType(DataType.Date)]
    public DateTime DownloadedAt { get; set; }
}
