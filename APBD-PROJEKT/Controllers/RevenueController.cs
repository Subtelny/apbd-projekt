using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
public class RevenueController(RevenueService revenueService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RevenueCalculation>> GetRevenue()
    {
        var revenue = await revenueService.CalculateRevenue();
        return Ok(revenue);
    }
}