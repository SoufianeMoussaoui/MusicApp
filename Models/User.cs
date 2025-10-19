using System.ComponentModel.DataAnnotations;

namespace musicApp.Models;

public class User
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Genre { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateCreationUser { get; set; }



}