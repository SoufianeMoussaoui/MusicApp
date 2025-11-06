using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using musicApp.Models;


using System.ComponentModel.DataAnnotations.Schema;
using Supabase.Postgrest.Models;

namespace musicApp.Models;

[Table("notifications")]
public class Notifications : BaseModel
{
    [Column("notification_id")]
    public int NotificationId { get; set; }
    
    [Column("user_id")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "The message is required")]
    [Column("content")]
    public string? Content { get; set; }

    [Column("count")]
    public int Count { get; set; } = 0;
    
    [Column("is_read")]
    public bool IsRead { get; set; } = false;    
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}