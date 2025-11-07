// Artist.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;



namespace musicApp.Models;

[Table("artists")]
public class Artist : User
{
    [Column("artist_id")]
    public int ArtistId { get; set; }
    
    [Column("links")]
    public List<string> Links { get; set; } = new List<string>();
    
    [Column("bio")]
    public string? Bio { get; set; }

}