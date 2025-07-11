using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class ApplicationTools
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ApplicationTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [McpServerTool, Description("Get all applications")]
    public async Task<List<Application>> GetAllApplicationsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return (List<Application>)await applicationService.GetAllAsync();
    }

    [McpServerTool, Description("Get applications by country ID")]
    public async Task<List<Application>> GetApplicationsByCountryIdAsync(
        [Description("The ID of the country")] int countryId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return (List<Application>)await applicationService.GetByCountryIdAsync(countryId);
    }

    [McpServerTool, Description("Get applications by product ID")]
    public async Task<List<Application>> GetApplicationsByProductIdAsync(
        [Description("The ID of the product")] int productId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return (List<Application>)await applicationService.GetByProductIdAsync(productId);
    }

    [McpServerTool, Description("Create a new application")]
    public async Task<Application> CreateApplicationAsync(
        [Description("The serial number of the application")] string serialNum,
        [Description("Required: Ask user if not entered: The name of the application")] string name,
        [Description("Required: Ask user if not entered: The ID of the country this application belongs to")] int countryId,
        [Description("Required: Ask user if not entered: Array of product IDs to associate with this application (comma-separated)")] string productIds,
        [Description("The type of the application")] string type = "",
        [Description("The application number")] string appNumber = "",
        [Description("Required: Ask user if not entered: The status of the application")] string status = "",
        [Description("The status date of the application (ISO format)")] string? statusDate = null)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();

        var application = new Application
        {
            SerialNum = serialNum,
            Name = name,
            CountryId = countryId,
            Type = type,
            AppNumber = appNumber,
            Status = status,
            StatusDate = string.IsNullOrEmpty(statusDate) ? null : DateTime.Parse(statusDate).ToUniversalTime()
        };

        // Parse product IDs from comma-separated string
        var productIdList = new List<int>();
        if (!string.IsNullOrWhiteSpace(productIds))
        {
            productIdList = productIds.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => int.Parse(s.Trim()))
                .ToList();
        }

        return await applicationService.CreateAsync(application, productIdList);
    }

    [McpServerTool, Description("Update an existing application")]
    public async Task<Application?> UpdateApplicationAsync(
        [Description("The ID of the application to update")] int id,
        [Description("The new serial number of the application")] string serialNum,
        [Description("The new name of the application")] string name,
        [Description("The new country ID")] int countryId,
        [Description("Optional: Product IDs to associate with this application (comma-separated). Leave empty to keep existing products.")] string? productIds = null,
        [Description("The new type of the application")] string type = "",
        [Description("The new application number")] string appNumber = "",
        [Description("The new status of the application")] string status = "",
        [Description("The new status date (ISO format)")] string? statusDate = null,
        [Description("Whether the application should be active")] bool isActive = true)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();

        var application = new Application
        {
            Id = id,
            SerialNum = serialNum,
            Name = name,
            CountryId = countryId,
            Type = type,
            AppNumber = appNumber,
            Status = status,
            StatusDate = string.IsNullOrEmpty(statusDate) ? null : DateTime.Parse(statusDate).ToUniversalTime(),
            IsActive = isActive
        };

        // Parse product IDs from comma-separated string if provided
        List<int>? productIdList = null;
        if (!string.IsNullOrWhiteSpace(productIds))
        {
            productIdList = productIds.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => int.Parse(s.Trim()))
                .ToList();
        }

        return await applicationService.UpdateAsync(application, productIdList);
    }

    [McpServerTool, Description("Delete an application (soft delete - marks as inactive)")]
    public async Task<bool> DeleteApplicationAsync(
        [Description("The ID of the application to delete")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return await applicationService.DeleteAsync(id);
    }

    [McpServerTool, Description("Associate a product with an application")]
    public async Task<bool> AddProductToApplicationAsync(
        [Description("The ID of the application")] int applicationId,
        [Description("The ID of the product to associate")] int productId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return await applicationService.AddProductToApplicationAsync(applicationId, productId);
    }

    [McpServerTool, Description("Remove a product association from an application")]
    public async Task<bool> RemoveProductFromApplicationAsync(
        [Description("The ID of the application")] int applicationId,
        [Description("The ID of the product to remove")] int productId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return await applicationService.RemoveProductFromApplicationAsync(applicationId, productId);
    }

    [McpServerTool, Description("Add multiple products to an application")]
    public async Task<bool> AddProductsToApplicationAsync(
        [Description("The ID of the application")] int applicationId,
        [Description("Comma-separated list of product IDs to add")] string productIds)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();

        var productIdList = productIds.Split(',')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => int.Parse(s.Trim()))
            .ToList();

        return await applicationService.AddProductsToApplicationAsync(applicationId, productIdList);
    }

    [McpServerTool, Description("Remove all products from an application")]
    public async Task<bool> RemoveAllProductsFromApplicationAsync(
        [Description("The ID of the application")] int applicationId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return await applicationService.RemoveAllProductsFromApplicationAsync(applicationId);
    }

    [McpServerTool, Description("Get all products associated with an application")]
    public async Task<List<Product>> GetApplicationProductsAsync(
        [Description("The ID of the application")] int applicationId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var applicationService = scope.ServiceProvider.GetRequiredService<IApplicationService>();
        return (List<Product>)await applicationService.GetApplicationProductsAsync(applicationId);
    }
}
