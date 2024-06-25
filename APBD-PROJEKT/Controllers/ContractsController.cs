using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using APBD_PROJEKT.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.controllers;

[Route("api/[controller]")]
[ApiController]
public class ContractsController(ContractService contractService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
    {
        var contracts = await contractService.GetAllContractsAsync();
        return Ok(contracts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contract>> GetContract(int id)
    {
        var contract = await contractService.GetContractByIdAsync(id);
        if (contract == null)
        {
            return NotFound();
        }
        return Ok(contract);
    }

    [HttpPost]
    public async Task<ActionResult<Contract>> PostContract(Contract contract)
    {
        try
        {
            var createdContract = await contractService.CreateContractAsync(contract);
            return CreatedAtAction(nameof(GetContract), new { id = createdContract.Id }, createdContract);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContract(int id)
    {
        try
        {
            var result = await contractService.DeleteContractAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}