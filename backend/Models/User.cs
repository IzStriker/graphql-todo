namespace Backend.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    [GraphQLIgnore]
    public string Password { get; set; } = string.Empty;

    public List<Task> Tasks { get; set; } = [];
}