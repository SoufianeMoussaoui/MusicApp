using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace musicApp.Models;

public class User
{
    public List<Notifications> Notifications { get; set; } = new List<Notifications>();

    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    
    [Required(ErrorMessage = "Username required")]
    [NotNull]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress]
    [NotNull]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is ")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [NotNull]
    public string? Password { get; set; }
    public string? Genre { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }

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

}