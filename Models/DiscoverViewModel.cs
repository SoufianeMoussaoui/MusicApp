using System.Collections.Generic;

namespace musicApp.Models
{
    public class DiscoverViewModel
    {
        public List<Song> TrendingSongs { get; set; } = new List<Song>();
        public List<Album> TrendingAlbums { get; set; } = new List<Album>();
        public int UnreadNotifications { get; set; } = 0;
        public bool IsAuthenticated { get; set; } = false;
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
