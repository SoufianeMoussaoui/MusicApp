// Artist.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Supabase.Postgrest.Models;


namespace musicApp.Models;

[Table("artists")]
public class Artist: BaseModel
{
    
    [Column("artist_id")]
    public int ArtistId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("links")]
    public List<string> Links { get; set; } = new List<string>();
    
    [Column("bio")]
    public string? Bio { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}