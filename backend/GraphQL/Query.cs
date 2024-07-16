using System.Security.Claims;
using Backend.Models;
using HotChocolate.Authorization;

namespace Backend.GraphQL;

public class Query
{
    [Authorize]
    [UseProjection]
    public IQueryable<User> GetUsers([Service] TodoDbContext context) => context.Users;

    [Authorize]
    [HotChocolate.Data.UseFiltering]
    public IQueryable<Backend.Models.Task> GetTasks([Service] TodoDbContext context, ClaimsPrincipal claims)
    {
        var userId = claims.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception();
        return context.Task.Where(t => t.UserId == userId);
    }
}