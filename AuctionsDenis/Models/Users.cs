using System.ComponentModel.DataAnnotations;

namespace AuctionsProject.Models;

public class Users
{
    [Key]
    public int UserId { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string PasswordHash { get; set; }
}