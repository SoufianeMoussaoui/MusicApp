using musicApp.Models;


public interface ISupabaseService
{
    Task<List<Song>> GetAllSongsAsync();
    //Task<Song> GetSongAsync(int id);
}