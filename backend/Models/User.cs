using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models;
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    [GraphQLIgnore]
    public string Password { get; set; } = string.Empty;
    public List<Task> Tasks { get; set; } = [];
}