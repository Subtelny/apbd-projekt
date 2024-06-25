using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
public class DiscountsController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Discount>>> GetDiscounts()
    {
        return await context.Discounts.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Discount>> GetDiscount(int id)
    {
        var discount = await context.Discounts.FindAsync(id);
        if (discount == null)
        {
            return NotFound();
        }
        return discount;
    }

    [HttpPost]
    public async Task<ActionResult<Discount>> PostDiscount(Discount discount)
    {
        context.Discounts.Add(discount);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDiscount), new { id = discount.Id }, discount);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDiscount(int id, Discount discount)
    {
        if (id != discount.Id)
        {
            return BadRequest();
        }
        context.Entry(discount).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DiscountExists(id))
            {
                return NotFound();
            }
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDiscount(int id)
    {
        var discount = await context.Discounts.FindAsync(id);
        if (discount == null)
        {
            return NotFound();
        }
        context.Discounts.Remove(discount);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private bool DiscountExists(int id)
    {
        return context.Discounts.Any(e => e.Id == id);
    }
}