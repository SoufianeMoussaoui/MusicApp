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
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress]
    [Column("email")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [Column("password_hash")]
    public string? PasswordHash { get; set; }
    
    [Column("genre")]
    public string? Genre { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    /*
    public List<Notifications> Notifications { get; set; } = new List<Notifications>();

    public int CountAllNotifications()
    {
        return Notifications?.Count ?? 0;
    }
    
    public int CountUnreadNotifications()
    {
        return Notifications?.Count(n => !n.IsRead) ?? 0;
    }
    
    public int CountReadNotifications()
    {
        return Notifications?.Count(n => n.IsRead) ?? 0;
    }
    */
}