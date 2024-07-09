namespace Backend.GraphQL.Models;

public class LoginRes
{

    public string Token { get; set; } = string.Empty;
    public DateTime ValidTill { get; set; }
}