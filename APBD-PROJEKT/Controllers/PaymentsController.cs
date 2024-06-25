using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController(PaymentService paymentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment?>>> GetPayments()
    {
        var payments = await paymentService.GetAllPaymentsAsync();
        return payments.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPayment(int id)
    {
        var payment = await paymentService.GetPaymentByIdAsync(id);
        if (payment == null)
        {
            return NotFound();
        }
        return payment;
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> PostPayment(Payment payment)
    {
        try
        {
            var createdPayment = await paymentService.CreatePaymentAsync(payment);
            return CreatedAtAction(nameof(GetPayment), new { id = createdPayment.Id }, createdPayment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var result = await paymentService.DeletePaymentAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}