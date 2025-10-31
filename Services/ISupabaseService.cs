using musicApp.Models;


public interface ISupabaseService
{
    Task<List<Song>> GetSongsAsync();
    Task<Song> GetSongAsync(int id);
}