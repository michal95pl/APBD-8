using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/trips")]
public class TripController : ControllerBase
{
    private readonly ITripService _tripService;
    private readonly IClientService _clientService;
    
    public TripController(ITripService tripService, IClientService clientService)
    {
        _tripService = tripService;
        _clientService = clientService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTrips()
    {
        
        var data = await _tripService.GetTrips();
        
        return Ok(data);
    }

    [HttpPost]
    [Route("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip(int idTrip, ClientTripDto clientTripDto)
    {

        if (!await _tripService.TripExists(idTrip))
            return NotFound("Trip not found");
        
        if (await _clientService.ClientPeselExists(clientTripDto.Pesel!))
        {
            if (await _clientService.HasClientTrip(await _clientService.GetClientId(clientTripDto.Pesel!))) 
                return Conflict("customer participates in this trip ");
        }
        else
        {
            await _clientService.AddClient(new Client()
                     {
                         FirstName = clientTripDto.FirstName!,
                         LastName = clientTripDto.LastName!,
                         Email = clientTripDto.Email!,
                         Telephone = clientTripDto.Telephone!,
                         Pesel = clientTripDto.Pesel!
                     });
        }

        var idClient = await _clientService.GetClientId(clientTripDto.Pesel!);
        
        await _tripService.AddClientToTrip(idClient, idTrip, clientTripDto.PaymentDate);
        
        return NoContent();
    }
}