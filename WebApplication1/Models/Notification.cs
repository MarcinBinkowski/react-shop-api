namespace WebApplication1.Models;
using System.ComponentModel.DataAnnotations;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public bool IsRead { get; set; } = false;

    [Required]
    public int UserId { get; set; }
}