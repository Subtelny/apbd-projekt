using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
public class SoftwareController(SoftwareService softwareService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Software>>> GetSoftware()
    {
        var software = await softwareService.GetAllSoftwareAsync();
        return Ok(software);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Software>> GetSoftware(int id)
    {
        var software = await softwareService.GetSoftwareByIdAsync(id);
        if (software == null)
        {
            return NotFound();
        }
        return Ok(software);
    }

    [HttpPost]
    public async Task<ActionResult<Software>> PostSoftware(Software software)
    {
        var createdSoftware = await softwareService.CreateSoftwareAsync(software);
        return CreatedAtAction(nameof(GetSoftware), new { id = createdSoftware.Id }, createdSoftware);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSoftware(int id, Software software)
    {
        var result = await softwareService.UpdateSoftwareAsync(id, software);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSoftware(int id)
    {
        var result = await softwareService.DeleteSoftwareAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}