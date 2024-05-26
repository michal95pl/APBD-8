using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface ITripService
{
    public Task<List<Trip>> GetTrips();
    public Task<bool> TripExists(int idTrip);
    public Task AddClientToTrip(int idClient, int idTrip, DateTime? paymentDate);
}

public class TripService : ITripService
{
    private MasterContext _context;
    
    public TripService(MasterContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> GetTrips()
    {
   
        var data = await _context.Trips
            //.Include(t => t.ClientTrips)
            .OrderByDescending(d => d.DateFrom)
            .ToListAsync();

        return data;
    }

    public async Task<bool> TripExists(int idTrip)
    {
        var cnt = await _context.Trips.CountAsync(t => t.IdTrip == idTrip);

        return cnt > 0;
    }

    public async Task AddClientToTrip(int idClient, int idTrip, DateTime? paymentDate)
    {
        var newClientTrip = new ClientTrip()
        {
            IdClient = idClient,
            IdTrip = idTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = paymentDate
        };

        await _context.ClientTrips.AddAsync(newClientTrip);

        await _context.SaveChangesAsync();

    }
}