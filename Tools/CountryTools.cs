using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class CountryTools
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CountryTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [McpServerTool, Description("Get all countries")]
    public async Task<List<Country>> GetAllCountriesAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return (List<Country>)await countryService.GetAllCountriesAsync();
    }

    [McpServerTool, Description("Get a country by its ID")]
    public async Task<Country?> GetCountryByIdAsync(
        [Description("The ID of the country to retrieve")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return await countryService.GetCountryByIdAsync(id);
    }

    [McpServerTool, Description("Get a country by its code")]
    public async Task<Country?> GetCountryByCodeAsync(
        [Description("The 3-letter code of the country (e.g., USA, GBR, IND)")] string code)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return await countryService.GetCountryByCodeAsync(code);
    }

    [McpServerTool, Description("Create a new country")]
    public async Task<Country> CreateCountryAsync(
        [Description("The name of the country")] string name,
        [Description("The 3-letter code of the country")] string code)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

        var country = new Country
        {
            Name = name,
            Code = code.ToUpper(),
            IsActive = true
        };

        return await countryService.CreateCountryAsync(country);
    }

    [McpServerTool, Description("Update an existing country")]
    public async Task<Country?> UpdateCountryAsync(
        [Description("The ID of the country to update")] int id,
        [Description("The new name of the country")] string name,
        [Description("The new 3-letter code of the country")] string code,
        [Description("Whether the country should be active")] bool isActive = true)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();

        var country = new Country
        {
            Name = name,
            Code = code.ToUpper(),
            IsActive = isActive
        };

        return await countryService.UpdateCountryAsync(id, country);
    }

    [McpServerTool, Description("Delete a country (soft delete - marks as inactive)")]
    public async Task<bool> DeleteCountryAsync(
        [Description("The ID of the country to delete")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var countryService = scope.ServiceProvider.GetRequiredService<ICountryService>();
        return await countryService.DeleteCountryAsync(id);
    }
}