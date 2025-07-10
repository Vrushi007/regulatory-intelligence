using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public interface ICountryService
{
    Task<IEnumerable<Country>> GetAllCountriesAsync();
    Task<Country?> GetCountryByIdAsync(int id);
    Task<Country?> GetCountryByCodeAsync(string code);
    Task<Country> CreateCountryAsync(Country country);
    Task<Country?> UpdateCountryAsync(int id, Country country);
    Task<bool> DeleteCountryAsync(int id);
}

public class CountryService : ICountryService
{
    private readonly ApplicationDbContext _context;

    public CountryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await _context.Countries.Where(c => c.IsActive).ToListAsync();
    }

    public async Task<Country?> GetCountryByIdAsync(int id)
    {
        return await _context.Countries.FindAsync(id);
    }

    public async Task<Country?> GetCountryByCodeAsync(string code)
    {
        return await _context.Countries
            .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
    }

    public async Task<Country> CreateCountryAsync(Country country)
    {
        country.CreatedAt = DateTime.UtcNow;
        _context.Countries.Add(country);
        await _context.SaveChangesAsync();
        return country;
    }

    public async Task<Country?> UpdateCountryAsync(int id, Country country)
    {
        var existingCountry = await _context.Countries.FindAsync(id);
        if (existingCountry == null)
            return null;

        existingCountry.Name = country.Name;
        existingCountry.Code = country.Code;
        existingCountry.IsActive = country.IsActive;
        existingCountry.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingCountry;
    }

    public async Task<bool> DeleteCountryAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country == null)
            return false;

        // Soft delete by setting IsActive to false
        country.IsActive = false;
        country.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
