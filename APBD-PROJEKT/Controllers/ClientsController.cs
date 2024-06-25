using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ClientsController(ClientService clientService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        var clients = await clientService.GetAllClientsAsync();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(int id)
    {
        var client = await clientService.GetClientByIdAsync(id);
        if (client == null)
        {
            return NotFound();
        }

        return Ok(client);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Client>> PostClient(Client client)
    {
        var createdClient = await clientService.CreateClientAsync(client);
        return CreatedAtAction(nameof(GetClient), new { id = createdClient.Id }, createdClient);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PutClient(int id, Client client)
    {
        var result = await clientService.UpdateClientAsync(id, client);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await clientService.DeleteClientAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}