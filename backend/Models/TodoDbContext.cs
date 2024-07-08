using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class TodoDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Task> Task { get; set; }

    public TodoDbContext() : base() { }
    public TodoDbContext(DbContextOptions<TodoDbContext> opt) : base(opt) { }
}