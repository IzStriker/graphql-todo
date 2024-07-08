using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<TodoDbContext>(
    opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();


app.Run();
