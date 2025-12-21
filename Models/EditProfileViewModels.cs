
namespace musicApp.Models;

// User.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


public class EditProfileViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = "";

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Genre { get; set; }
}