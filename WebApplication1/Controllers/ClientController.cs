using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;
    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteClient([FromQuery] int idClient)
    {
        if (!await _clientService.ClientExists(idClient))
            return NotFound("Client not exists in database");
        
        if (await _clientService.HasClientTrip(idClient))
            return Conflict("Client has trip");

        await _clientService.DeleteClient(idClient);
        
        return NoContent();
    }
    
}