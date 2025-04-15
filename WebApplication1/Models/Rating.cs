namespace WebApplication1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [Range(1, 5)] public int Value { get; set; }

    [StringLength(500)] public string? Comment { get; set; }

    [Required] public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Required] public int ProductId { get; set; }

    [ForeignKey("ProductId")] public virtual Product? Product { get; set; }
}