using APBD_PROJEKT.database;
using APBD_PROJEKT.model;
using Microsoft.EntityFrameworkCore;

namespace APBD_PROJEKT.Service;

public class SoftwareService(ApplicationDbContext context)
{
    public async Task<IEnumerable<Software?>> GetAllSoftwareAsync()
    {
        return await context.Softwares.ToListAsync();
    }

    public async Task<Software?> GetSoftwareByIdAsync(int id)
    {
        return await context.Softwares.FindAsync(id);
    }

    public async Task<Software> CreateSoftwareAsync(Software software)
    {
        context.Softwares.Add(software);
        await context.SaveChangesAsync();
        return software;
    }

    public async Task<bool> UpdateSoftwareAsync(int id, Software software)
    {
        if (id != software.Id)
        {
            return false;
        }

        context.Entry(software).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SoftwareExists(id))
            {
                return false;
            }
            throw;
        }
    }

    public async Task<bool> DeleteSoftwareAsync(int id)
    {
        var software = await context.Softwares.FindAsync(id);
        if (software == null)
        {
            return false;
        }

        context.Softwares.Remove(software);
        await context.SaveChangesAsync();
        return true;
    }

    private bool SoftwareExists(int id)
    {
        return context.Softwares.Any(e => e.Id == id);
    }
}