using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id); Task<Product?> GetProductByCodeNameAsync(string codeName);
    Task<Product> CreateProductAsync(Product product);
    Task<Product?> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
    Task<IEnumerable<Product>> GetProductsByProductFamilyIdAsync(int productFamilyId);
    Task<IEnumerable<Product>> SearchProductsByNameAsync(string namePattern);

    // Methods for controlled vocabularies
    Task<IEnumerable<Product>> GetProductsByTypeIdAsync(int typeId);
    Task<IEnumerable<Product>> GetProductsBySubtypeIdAsync(int subtypeId);
    Task<IEnumerable<Product>> GetProductsByRiskIdAsync(int riskId);
    Task<IEnumerable<Product>> GetProductsByClassificationIdAsync(int classificationId);
    Task<Product?> GetProductWithVocabulariesAsync(int id);
}

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Where(p => p.IsActive)
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetProductByCodeNameAsync(string codeName)
    {
        return await _context.Products
            .Include(p => p.ProductFamily)
            .FirstOrDefaultAsync(p => p.CodeName == codeName && p.IsActive);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        // Validate that the ProductFamily exists
        var productFamily = await _context.ProductFamilies.FindAsync(product.ProductFamilyId);
        if (productFamily == null)
        {
            throw new InvalidOperationException($"Product Family with ID {product.ProductFamilyId} does not exist.");
        }

        product.CreatedAt = DateTime.UtcNow;
        product.IsActive = true;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Reload the product with ProductFamily included
        return await GetProductByIdAsync(product.Id) ?? product;
    }

    public async Task<Product?> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
            return null;

        // Validate that the ProductFamily exists if it's being changed
        if (existingProduct.ProductFamilyId != product.ProductFamilyId)
        {
            var productFamily = await _context.ProductFamilies.FindAsync(product.ProductFamilyId);
            if (productFamily == null)
            {
                throw new InvalidOperationException($"Product Family with ID {product.ProductFamilyId} does not exist.");
            }
        }

        existingProduct.Name = product.Name;
        existingProduct.CodeName = product.CodeName;
        existingProduct.Description = product.Description;

        // Handle vocabulary relationships
        existingProduct.RiskId = product.RiskId;
        existingProduct.ClassificationId = product.ClassificationId;
        existingProduct.TypeId = product.TypeId;
        existingProduct.SubtypeId = product.SubtypeId;

        existingProduct.ProductFamilyId = product.ProductFamilyId;
        existingProduct.IsActive = product.IsActive;
        existingProduct.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Reload the product with ProductFamily included
        return await GetProductByIdAsync(id);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        // Soft delete by setting IsActive to false
        product.IsActive = false;
        product.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Product>> GetProductsByProductFamilyIdAsync(int productFamilyId)
    {
        return await _context.Products
            .Where(p => p.ProductFamilyId == productFamilyId && p.IsActive)
            .Include(p => p.ProductFamily)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchProductsByNameAsync(string namePattern)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(namePattern) && p.IsActive)
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .ToListAsync();
    }

    // Methods for controlled vocabularies
    public async Task<IEnumerable<Product>> GetProductsByTypeIdAsync(int typeId)
    {
        return await _context.Products
            .Where(p => p.TypeId == typeId && p.IsActive)
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsBySubtypeIdAsync(int subtypeId)
    {
        return await _context.Products
            .Where(p => p.SubtypeId == subtypeId && p.IsActive)
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByRiskIdAsync(int riskId)
    {
        return await _context.Products
            .Where(p => p.RiskId == riskId && p.IsActive)
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByClassificationIdAsync(int classificationId)
    {
        return await _context.Products
            .Where(p => p.ClassificationId == classificationId && p.IsActive)
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .ToListAsync();
    }

    public async Task<Product?> GetProductWithVocabulariesAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProductFamily)
            .Include(p => p.RiskVocabulary)
            .Include(p => p.Classification)
            .Include(p => p.ProductType)
            .Include(p => p.ProductSubtype)
            .Include(p => p.Applications)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
