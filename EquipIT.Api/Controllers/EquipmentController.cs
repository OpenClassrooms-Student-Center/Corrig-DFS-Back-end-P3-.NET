using EquipIT.Api.Models;
using EquipIT.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EquipIT.Api.Controllers;

[ApiController]
[Route("api/equipment")]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;

    public EquipmentController(IEquipmentService equipmentService)
    {
        _equipmentService = equipmentService;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<Equipment>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var equipments = await _equipmentService.GetAllEquipmentAsync();
        return Ok(equipments);
    }

    [HttpPut("{id:int}/borrow")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Borrow(int id, [FromBody] BorrowRequest request)
    {
        try
        {
            await _equipmentService.BorrowEquipmentAsync(id, request.FirstName, request.LastName);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Item not found or already borrowed -> 400
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Return(int id)
    {
        try
        {
            await _equipmentService.ReturnEquipmentAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            // Item not found or not currently borrowed -> 400
            return BadRequest(ex.Message);
        }
    }
}
