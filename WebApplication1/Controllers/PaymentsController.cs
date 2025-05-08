using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
    {
        return Ok(await _context.Set<Payment>().ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPayment(int id)
    {
        var payment = await _context.Set<Payment>().FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }
        return Ok(payment);
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
    {
        _context.Set<Payment>().Add(payment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(int id, Payment updateDto)
    {
        var payment = await _context.Payments.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        payment.PaymentMethod = updateDto.PaymentMethod;
        payment.Amount = updateDto.Amount;
        payment.PaymentDate = updateDto.PaymentDate;
        payment.Notes = updateDto.Notes;
        payment.OrderId = updateDto.OrderId;


        await _context.SaveChangesAsync();
        return Ok(payment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _context.Set<Payment>().FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        _context.Set<Payment>().Remove(payment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpGet("user/{customerName}")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByCustomerName(string customerName)
    {
        var orders = await _context.Orders
            .Where(o => o.CustomerName.Contains(customerName))
            .Select(o => o.Id)
            .ToListAsync();

        var payments = await _context.Payments
            .Where(p => orders.Contains(p.OrderId))
            .OrderByDescending(p => p.PaymentDate)
            .ToListAsync();

        return Ok(payments);
    }
}