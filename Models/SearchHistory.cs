using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace musicApp.Models;

[Table("search_history")]
public class SearchHistory
{
    [Key]
    [Column("search_history_id")]
    public int SearchHistoryId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; } 

    [Required]
    [Column("search_term")]
    public string SearchTerm { get; set; } = "";

    [Column("search_type")]
    public string SearchType { get; set; } = "song"; 

    [Column("result_count")]
    public int ResultCount { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("session_id")]
    public string? SessionId { get; set; } 

    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
