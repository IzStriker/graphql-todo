using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Task
{
    [Key]
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public string UserId { get; set; } = string.Empty;
}