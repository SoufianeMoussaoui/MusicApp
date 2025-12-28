using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace musicApp.Models
{
    public class DiscoverViewModel
    {
        public int Id {get; set;}
        public List<Song> TrendingSongs { get; set; } = new List<Song>();
        public List<Album> TrendingAlbums { get; set; } = new List<Album>();
        public int UnreadNotifications { get; set; } = 0;
        public bool IsAuthenticated { get; set; } = false;
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string SearchTerm { get; set; } = "";
        public List<SearchHistory> RecentSearches { get; set; } = new List<SearchHistory>(); 
        public List<string> AvailableGenres { get; set; } = new List<string>();

        public string CurrentFilter { get; set; } = "home"; // Add this
        public string? CurrentGenre { get; set; } // Add this
    }
}
