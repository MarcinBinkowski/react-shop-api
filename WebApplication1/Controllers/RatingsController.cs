using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RatingsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Rating>>> GetRatings()
    {
        return await _context.Ratings.Include(r => r.Product).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Rating>> GetRating(int id)
    {
        var rating = await _context.Ratings
            .Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rating == null)
        {
            return NotFound();
        }

        return rating;
    }

    [HttpGet("product/{productId}/average")]
    public async Task<ActionResult<double>> GetAverageRating(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        var average = await _context.Ratings
            .Where(r => r.ProductId == productId)
            .AverageAsync(r => r.Value);

        return Ok(new { productId, averageRating = Math.Round(average, 2) });
    }

    [HttpPost]
    public async Task<ActionResult<Rating>> CreateRating(Rating rating)
    {
        if (!await _context.Products.AnyAsync(p => p.Id == rating.ProductId))
        {
            return BadRequest("Invalid ProductId");
        }

        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRating), new { id = rating.Id }, rating);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRating(int id, Rating rating)
    {
        if (id != rating.Id)
        {
            return BadRequest();
        }

        var existingRating = await _context.Ratings.FindAsync(id);
        if (existingRating == null)
        {
            return NotFound();
        }

        existingRating.Value = rating.Value;
        existingRating.Comment = rating.Comment;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Ratings.AnyAsync(r => r.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRating(int id)
    {
        var rating = await _context.Ratings.FindAsync(id);
        if (rating == null)
        {
            return NotFound();
        }

        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}