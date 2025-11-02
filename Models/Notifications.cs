using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using musicApp.Models;
using Postgrest.Models;

public class Notifications : BaseModel
{
    public int Id {get; set;}

    [NotNull]
    [Required(ErrorMessage = "the message is reqired")]
    public string? Content {get; set;}

    public int Count {get; set;} = 0;
    public bool IsRead  {get; set;} = false;    
    public DateTime CreatedAt {get; set;}

}