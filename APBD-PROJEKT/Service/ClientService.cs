using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.Service;

public class ClientService(ApplicationDbContext context)
{
    public async Task<IEnumerable<Client?>> GetAllClientsAsync()
    {
        return await context.Clients.ToListAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int id)
    {
        return await context.Clients.FindAsync(id);
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        context.Clients.Add(client);
        await context.SaveChangesAsync();
        return client;
    }

    public async Task<bool> UpdateClientAsync(int id, Client client)
    {
        if (id != client.Id)
        {
            return false;
        }

        context.Entry(client).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClientExists(id))
            {
                return false;
            }
            throw;
        }
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await context.Clients.FindAsync(id);
        switch (client)
        {
            case null:
                return false;
            case IndividualClient:
                context.Clients.Remove(client);
                break;
            case CompanyClient:
                return false;
        }

        await context.SaveChangesAsync();
        return true;
    }

    private bool ClientExists(int id)
    {
        return context.Clients.Any(e => e.Id == id);
    }
}