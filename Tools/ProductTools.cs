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

    // [McpServerTool, Description("Get a product by its code name")]
    // public async Task<Product?> GetProductByCodeNameAsync(
    //     [Description("The code name of the product to retrieve")] string codeName)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
    //     return await productService.GetProductByCodeNameAsync(codeName);
    // }

    // [McpServerTool, Description("Search products by name (partial match)")]
    // public async Task<List<Product>> SearchProductsByNameAsync(
    //     [Description("Part of the product name to search for")] string namePattern)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
    //     return (List<Product>)await productService.SearchProductsByNameAsync(namePattern);
    // }

    // [McpServerTool, Description("Get products by product family ID")]
    // public async Task<List<Product>> GetProductsByProductFamilyIdAsync(
    //     [Description("The ID of the product family")] int productFamilyId)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
    //     return (List<Product>)await productService.GetProductsByProductFamilyIdAsync(productFamilyId);
    // }

    // [McpServerTool, Description("Get products by type")]
    // public async Task<List<Product>> GetProductsByTypeAsync(
    //     [Description("The type of products to retrieve")] string type)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
    //     return (List<Product>)await productService.GetProductsByTypeAsync(type);
    // }

    // [McpServerTool, Description("Get products by risk level")]
    // public async Task<List<Product>> GetProductsByRiskAsync(
    //     [Description("The risk level of products to retrieve")] string risk)
    // {
    //     using var scope = _serviceScopeFactory.CreateScope();
    //     var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
    //     return (List<Product>)await productService.GetProductsByRiskAsync(risk);
    // }

    [McpServerTool, Description("Create a new product with controlled vocabularies")]
    public async Task<Product> CreateProductAsync(
        [Description("The name of the product")] string name,
        [Description("The code name of the product")] string codeName,
        [Description("The description of the product")] string description,
        [Description("The ID of the product family this product belongs to")] int productFamilyId,
        [Description("The risk level ID (optional)")] int? riskId = null,
        [Description("The classification ID (optional)")] int? classificationId = null,
        [Description("The product type ID (optional)")] int? typeId = null,
        [Description("The product subtype ID (optional)")] int? subtypeId = null)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

        var product = new Product
        {
            Name = name,
            CodeName = codeName,
            Description = description,
            ProductFamilyId = productFamilyId,
            RiskId = riskId,
            ClassificationId = classificationId,
            TypeId = typeId,
            SubtypeId = subtypeId,
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
        [Description("The risk level ID (optional)")] int? riskId = null,
        [Description("The classification ID (optional)")] int? classificationId = null,
        [Description("The product type ID (optional)")] int? typeId = null,
        [Description("The product subtype ID (optional)")] int? subtypeId = null,
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
            RiskId = riskId,
            ClassificationId = classificationId,
            TypeId = typeId,
            SubtypeId = subtypeId,
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

    [McpServerTool, Description("Get a product with all its vocabulary relationships")]
    public async Task<Product?> GetProductWithVocabulariesAsync(
        [Description("The ID of the product to retrieve")] int id)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return await productService.GetProductWithVocabulariesAsync(id);
    }

    [McpServerTool, Description("Get products by risk level ID")]
    public async Task<List<Product>> GetProductsByRiskIdAsync(
        [Description("The risk level ID to filter by")] int riskId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return (List<Product>)await productService.GetProductsByRiskIdAsync(riskId);
    }

    [McpServerTool, Description("Get products by classification ID")]
    public async Task<List<Product>> GetProductsByClassificationIdAsync(
        [Description("The classification ID to filter by")] int classificationId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return (List<Product>)await productService.GetProductsByClassificationIdAsync(classificationId);
    }

    [McpServerTool, Description("Get products by product type ID")]
    public async Task<List<Product>> GetProductsByTypeIdAsync(
        [Description("The product type ID to filter by")] int typeId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return (List<Product>)await productService.GetProductsByTypeIdAsync(typeId);
    }

    [McpServerTool, Description("Get products by product subtype ID")]
    public async Task<List<Product>> GetProductsBySubtypeIdAsync(
        [Description("The product subtype ID to filter by")] int subtypeId)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        return (List<Product>)await productService.GetProductsBySubtypeIdAsync(subtypeId);
    }
}
