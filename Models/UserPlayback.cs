using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace musicApp.Models;


public class UserPlayback
{
    public int UserPlaybackId { get; set; }
    public int UserId { get; set; }
    public int SongId { get; set; }
    [NotNull]
    public int CurrentPosition { get; set; }

    [DataType(DataType.Date)]
    public DateTime LastPlayed { get; set; }

}