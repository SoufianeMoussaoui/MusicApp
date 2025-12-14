// User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace musicApp.Models;

[Table("users")]
public class User  
{
    [Key]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [Column("first_name")]
    public string? Firstname { get; set; }
    
    [Column("last_name")]
    public string? Lastname { get; set; }
    
    [Required(ErrorMessage = "Username required")]
    [Column("username")]
    [NotNull]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress]
    [Column("email")]
    [NotNull]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [Column("password_hash")]
    public string? PasswordHash { get; set; }
    
    [Column("total_seconds_played")]
    public long TotalSecondsPlayed { get; set; } = 0;
    
    [Column("genre")]
    public string? Genre { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("is_admin")]
    public bool IsAdmin { get; set; } = false;
    
    [Column("is_active")]
    public bool IsActive { get; set; } = true;
}