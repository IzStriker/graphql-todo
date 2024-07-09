using Backend.Models;
using HotChocolate.Authorization;

namespace Backend.GraphQL;

public class Query
{
    [Authorize]

    public IQueryable<User> GetUsers([Service] TodoDbContext context) => context.Users;
}