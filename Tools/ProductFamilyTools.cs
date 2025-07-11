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
}
