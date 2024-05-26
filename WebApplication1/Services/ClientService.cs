using Microsoft.EntityFrameworkCore;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Services;

public interface IClientService
{
    public Task DeleteClient(int idClient);
    public Task<bool> HasClientTrip(int idClient);
    public Task<bool> HasClientTrip(int idClient, int idTrip);
    public Task<int> GetClientId(string pesel);
    public Task<bool> ClientExists(int idClient);
    public Task<bool> ClientPeselExists(string pesel);
    public Task AddClient(Client client);
}

public class ClientService : IClientService
{
    private readonly MasterContext _context;
    public ClientService(MasterContext context)
    {
        _context = context;
    }
    
    
    public async Task DeleteClient(int idClient)
    {
        var clientToRemove = new Client()
        {
            IdClient = idClient
        };

        _context.Clients.Attach(clientToRemove);
        
        _context.Clients.Remove(clientToRemove);

        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> HasClientTrip(int idClient)
    {
        var tripCount = await _context.ClientTrips
            .CountAsync(c => c.IdClient == idClient);

        return tripCount > 0;
    }

    public async Task<bool> ClientExists(int idClient)
    {
        var exists = await _context.Clients.CountAsync(c => c.IdClient == idClient);
        return exists > 0;
    }

    public async Task<bool> ClientPeselExists(string pesel)
    {
        var exists = await _context.Clients.CountAsync(c => c.Pesel == pesel);

        return exists > 0;
    }

    public async Task AddClient(Client client)
    {
        await _context.Clients.AddAsync(client);

        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasClientTrip(int idClient, int idTrip)
    {
        var cnt = await _context.ClientTrips.CountAsync(ct => ct.IdClient == idClient && ct.IdTrip == idTrip);

        return cnt > 0;
    }

    public async Task<int> GetClientId(string pesel)
    {
        var id = await _context.Clients.SingleAsync(c => c.Pesel == pesel);

        return id.IdClient;
    }
}