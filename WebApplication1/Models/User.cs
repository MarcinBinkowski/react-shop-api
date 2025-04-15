namespace WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100, MinimumLength = 3)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Address { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [Required]
    public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

    [Required]
    public bool IsAdmin { get; set; }
}