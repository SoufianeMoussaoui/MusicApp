using musicApp.Models;

namespace musicApp.Services
{
    public interface ISupabaseService
    {
        Task<List<Song>> GetAllSongsAsync();
        Task<List<Album>> GetAllAlbumsAsync();
    }
}

