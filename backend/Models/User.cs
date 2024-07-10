using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public string Id { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [GraphQLIgnore]
    public string Password { get; set; } = string.Empty;
    [GraphQLIgnore]
    public byte[] PasswordSalt { get; set; } = [];
    public List<Task> Tasks { get; set; } = [];
}