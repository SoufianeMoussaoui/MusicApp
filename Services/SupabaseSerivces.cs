using musicApp.Models;
using Supabase;
using Postgrest.Models;

public class SupabaseService : ISupabaseService
{
    private readonly Supabase.Client _client;

    public SupabaseService(Supabase.Client client)
    {
        _client = client;
    }

    public async Task<List<Song>> GetAllSongsAsync()
    {
        try
        {
            var respone = await _client
                .From<Song>()
                .Get();
                
            return respone.Models;
        }
        catch(Exception e)
        {
            Console.WriteLine($"Error fetching data: {e.Message}");
            return new List<Song>();
        }
        
    }


    public async Task<Song> GetSongAsync(int id)
    {
        var response = await _client
            .From<Song>()
            .Where(x => x.SongId == id)
            .Single();
        
        return response;
    }
}