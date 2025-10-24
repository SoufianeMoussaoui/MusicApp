using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace musicApp.Models;

public class User
{
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

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [NotNull]
    public string? Password { get; set; }
    public string? Genre { get; set; }

    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }



}