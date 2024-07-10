using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Task
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public int UserId { get; set; }
}