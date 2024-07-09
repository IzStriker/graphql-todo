using Backend.GraphQL;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<TodoDbContext>(
    opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
);


builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<TodoDbContext>()
    .AddQueryType<Query>();

var app = builder.Build();

app.MapGraphQL();

app.Run();
