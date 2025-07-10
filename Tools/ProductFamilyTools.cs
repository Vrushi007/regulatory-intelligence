using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class ProductFamilyTools
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductFamilyTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [McpServerTool, Description("Get all product families")]
    public async Task<List<ProductFamily>> GetAllProductFamiliesAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();
        return (List<ProductFamily>)await productFamilyService.GetAllProductFamiliesAsync();
    }

    [McpServerTool, Description("Get a product family by its ID")]
    public async Task<ProductFamily?> GetProductFamilyByIdAsync(
        [Description("The ID of the product family to retrieve")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();
        return await productFamilyService.GetProductFamilyByIdAsync(id);
    }

    // [McpServerTool, Description("Get a product family by its name")]
    // public async Task<ProductFamily?> GetProductFamilyByNameAsync(
    //     [Description("The name of the product family to retrieve")] string name)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();
    //     return await productFamilyService.GetProductFamilyByNameAsync(name);
    // }

    // [McpServerTool, Description("Search product families by name (partial match)")]
    // public async Task<List<ProductFamily>> SearchProductFamiliesByNameAsync(
    //     [Description("Part of the product family name to search for")] string namePattern)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();
    //     return (List<ProductFamily>)await productFamilyService.SearchProductFamiliesByNameAsync(namePattern);
    // }

    // [McpServerTool, Description("Get product families by type")]
    // public async Task<List<ProductFamily>> GetProductFamiliesByTypeAsync(
    //     [Description("The type of product families to retrieve")] string type)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();
    //     return (List<ProductFamily>)await productFamilyService.GetProductFamiliesByTypeAsync(type);
    // }

    [McpServerTool, Description("Create a new product family")]
    public async Task<ProductFamily> CreateProductFamilyAsync(
        [Description("The name of the product family")] string name,
        [Description("The description of the product family")] string description,
        [Description("The type of the product family")] string type)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();

        var productFamily = new ProductFamily
        {
            Name = name,
            Description = description,
            Type = type,
            IsActive = true
        };

        return await productFamilyService.CreateProductFamilyAsync(productFamily);
    }

    [McpServerTool, Description("Update an existing product family")]
    public async Task<ProductFamily?> UpdateProductFamilyAsync(
        [Description("The ID of the product family to update")] int id,
        [Description("The new name of the product family")] string name,
        [Description("The new description of the product family")] string description,
        [Description("The new type of the product family")] string type,
        [Description("Whether the product family should be active")] bool isActive = true)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();

        var productFamily = new ProductFamily
        {
            Name = name,
            Description = description,
            Type = type,
            IsActive = isActive
        };

        return await productFamilyService.UpdateProductFamilyAsync(id, productFamily);
    }

    [McpServerTool, Description("Delete a product family (soft delete - marks as inactive)")]
    public async Task<bool> DeleteProductFamilyAsync(
        [Description("The ID of the product family to delete")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();

        try
        {
            return await productFamilyService.DeleteProductFamilyAsync(id);
        }
        catch (InvalidOperationException)
        {
            // Return false if deletion fails due to business rules (e.g., has active products)
            return false;
        }
    }

    // [McpServerTool, Description("Get product families with advanced filtering")]
    // public async Task<List<ProductFamily>> GetProductFamiliesWithFilterAsync(
    //     [Description("Filter by active status (true for active, false for inactive, null for all)")] bool? isActive = null,
    //     [Description("Maximum number of results to return")] int? limit = null,
    //     [Description("Skip this many results (for pagination)")] int? skip = null)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productFamilyService = scope.ServiceProvider.GetRequiredService<IProductFamilyService>();

    //     List<ProductFamily> productFamilies;

    //     if (isActive.HasValue && !isActive.Value)
    //     {
    //         // For inactive product families, we need to query the database directly
    //         // since the service only returns active product families
    //         var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //         productFamilies = await context.ProductFamilies
    //             .Where(pf => !pf.IsActive)
    //             .Include(pf => pf.Products)
    //             .ToListAsync();
    //     }
    //     else if (isActive == null)
    //     {
    //         // Get all product families (active and inactive)
    //         var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //         productFamilies = await context.ProductFamilies
    //             .Include(pf => pf.Products)
    //             .ToListAsync();
    //     }
    //     else
    //     {
    //         // Get active product families (default behavior)
    //         productFamilies = (await productFamilyService.GetAllProductFamiliesAsync()).ToList();
    //     }

    //     // Apply pagination
    //     if (skip.HasValue)
    //         productFamilies = productFamilies.Skip(skip.Value).ToList();

    //     if (limit.HasValue)
    //         productFamilies = productFamilies.Take(limit.Value).ToList();

    //     return productFamilies;
    // }
}
