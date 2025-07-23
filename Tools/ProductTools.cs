using System;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using RimPoc.Data;
using RimPoc.Services;

namespace RimPoc.Tools;

[McpServerToolType]
public class ProductTools
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ProductTools(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    [McpServerTool, Description("Get all products")]
    public async Task<List<Product>> GetAllProductsAsync()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return (List<Product>)await productService.GetAllProductsAsync();
    }

    [McpServerTool, Description("Get a product by its ID")]
    public async Task<Product?> GetProductByIdAsync(
        [Description("The ID of the product to retrieve")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return await productService.GetProductByIdAsync(id);
    }


    [McpServerTool, Description("Create a new product with controlled vocabularies")]
    public async Task<Product> CreateProductAsync(
        [Description("Required: (Ask user if not provided): The name of the product")] string name,
        [Description("Required: (Ask user if not provided): The code name of the product")] string codeName,
        [Description("Required: (Ask user if not provided): The description of the product")] string description,
        [Description("Required: (Ask user if not provided): The ID of the product family this product belongs to")] int productFamilyId,
        [Description("Required: (Ask user if not provided): The medical specialty ID")] int? medicalSpecialtyId = null,
        [Description("Required: (Ask user if not provided): The product type ID ")] int? typeId = null,
        [Description("Required: (Ask user if not provided): The product subtype ID ")] int? subtypeId = null,
        [Description("Required: (Ask user if not provided): The functions ID ")] int? functionsId = null,
        [Description("Required: (Ask user if not provided): The energy source ID ")] int? energySourceId = null,
        [Description("Required: (Ask user if not provided): The radiation type ID ")] int? radiationTypeId = null,
        [Description("Required: (Ask user if not provided): Whether the product emits radiation")] bool radiationEmitting = false)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

        var product = new Product
        {
            Name = name,
            CodeName = codeName,
            Description = description,
            ProductFamilyId = productFamilyId,
            MedicalSpecialtyId = medicalSpecialtyId,
            TypeId = typeId,
            SubtypeId = subtypeId,
            FunctionsId = functionsId,
            EnergySourceId = energySourceId,
            RadiationTypeId = radiationTypeId,
            RadiationEmitting = radiationEmitting,
            IsActive = true
        };

        return await productService.CreateProductAsync(product);
    }

    [McpServerTool, Description("Update an existing product with controlled vocabularies")]
    public async Task<Product?> UpdateProductAsync(
        [Description("The ID of the product to update")] int id,
        [Description("The new name of the product")] string name,
        [Description("The new code name of the product")] string codeName,
        [Description("The new description of the product")] string description,
        [Description("The new product family ID")] int productFamilyId,
        [Description("The medical specialty ID (optional)")] int? medicalSpecialtyId = null,
        [Description("The product type ID (optional)")] int? typeId = null,
        [Description("The product subtype ID (optional)")] int? subtypeId = null,
        [Description("The functions ID (optional)")] int? functionsId = null,
        [Description("The energy source ID (optional)")] int? energySourceId = null,
        [Description("The radiation type ID (optional)")] int? radiationTypeId = null,
        [Description("Whether the product emits radiation")] bool radiationEmitting = false,
        [Description("Whether the product should be active")] bool isActive = true)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

        var product = new Product
        {
            Name = name,
            CodeName = codeName,
            Description = description,
            ProductFamilyId = productFamilyId,
            MedicalSpecialtyId = medicalSpecialtyId,
            TypeId = typeId,
            SubtypeId = subtypeId,
            FunctionsId = functionsId,
            EnergySourceId = energySourceId,
            RadiationTypeId = radiationTypeId,
            RadiationEmitting = radiationEmitting,
            IsActive = isActive
        };

        return await productService.UpdateProductAsync(id, product);
    }

    [McpServerTool, Description("Delete a product (soft delete - marks as inactive)")]
    public async Task<bool> DeleteProductAsync(
        [Description("The ID of the product to delete")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return await productService.DeleteProductAsync(id);
    }
}
