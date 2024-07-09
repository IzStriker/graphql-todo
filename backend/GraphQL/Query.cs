using Backend.Models;

namespace Backend.GraphQL;

public class Query
{
    public IQueryable<User> GetUsers([Service] TodoDbContext context) => context.Users;
}