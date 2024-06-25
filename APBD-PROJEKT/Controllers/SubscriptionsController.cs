using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController(SubscriptionService subscriptionService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subscription?>>> GetSubscriptions()
    {
        var subscriptions = await subscriptionService.GetAllSubscriptionsAsync();
        return subscriptions;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Subscription>> GetSubscription(int id)
    {
        var subscription = await subscriptionService.GetSubscriptionByIdAsync(id);

        if (subscription == null)
        {
            return NotFound();
        }
        return subscription;
    }

    [HttpPost]
    public async Task<ActionResult<Subscription>> PostSubscription(Subscription subscription)
    {
        try
        {
            var createdSubscription = await subscriptionService.CreateSubscriptionAsync(subscription);
            return CreatedAtAction(nameof(GetSubscription), new { id = createdSubscription.Id }, createdSubscription);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubscription(int id, Subscription subscription)
    {
        if (id != subscription.Id)
        {
            return BadRequest();
        }
        var result = await subscriptionService.UpdateSubscriptionAsync(id, subscription);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        var result = await subscriptionService.DeleteSubscriptionAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}