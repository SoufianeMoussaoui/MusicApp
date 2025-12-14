// Models/AdminViewModels.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace musicApp.Models
{
    // For Admin Dashboard
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalSongs { get; set; }
        public int TotalPlaylists { get; set; }
        public int TotalDownloads { get; set; }
        public List<User> RecentUsers { get; set; } = new List<User>();
        public List<Song> RecentSongs { get; set; } = new List<Song>();
    }

    // For Admin Users Page
    public class AdminUsersViewModel
    {
        public List<User> Users { get; set; } = new List<User>();
        public string SearchTerm { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
    }

    // For Admin User Details
    public class AdminUserDetailsViewModel
    {
        public User User { get; set; } = new User();
        public UserStatistics Statistics { get; set; } = new UserStatistics();
    }

    // User Statistics
    public class UserStatistics
    {
        public int TotalPlays { get; set; } = 0;
        public int TotalPlaylists { get; set; } = 0;
        public int TotalDownloads { get; set; } = 0;
        public long TotalSecondsPlayed { get; set; } = 0;
    }

    // For Admin Songs Page
    public class AdminSongsViewModel
    {
        public List<Song> Songs { get; set; } = new List<Song>();
        public string SearchTerm { get; set; } = "";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 0;
    }

    // For Edit User
    public class EditUserViewModel
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
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }

    // For Delete Confirmation
    public class DeleteConfirmationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; // "User" or "Song"
        public DateTime CreatedAt { get; set; }
    }
}