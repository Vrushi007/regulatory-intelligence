using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public interface IProductFamilyService
{
    Task<IEnumerable<ProductFamily>> GetAllProductFamiliesAsync();
    Task<ProductFamily?> GetProductFamilyByIdAsync(int id);
    Task<ProductFamily?> GetProductFamilyByNameAsync(string name);
    Task<ProductFamily> CreateProductFamilyAsync(ProductFamily productFamily);
    Task<ProductFamily?> UpdateProductFamilyAsync(int id, ProductFamily productFamily);
    Task<bool> DeleteProductFamilyAsync(int id);
    Task<IEnumerable<ProductFamily>> GetProductFamiliesByTypeAsync(string type);
    Task<IEnumerable<ProductFamily>> SearchProductFamiliesByNameAsync(string namePattern);
}

public class ProductFamilyService : IProductFamilyService
{
    private readonly ApplicationDbContext _context;

    public ProductFamilyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductFamily>> GetAllProductFamiliesAsync()
    {
        return await _context.ProductFamilies
            .Where(pf => pf.IsActive)
            .Include(pf => pf.Products)
            .ToListAsync();
    }

    public async Task<ProductFamily?> GetProductFamilyByIdAsync(int id)
    {
        return await _context.ProductFamilies
            .Include(pf => pf.Products)
            .FirstOrDefaultAsync(pf => pf.Id == id);
    }

    public async Task<ProductFamily?> GetProductFamilyByNameAsync(string name)
    {
        return await _context.ProductFamilies
            .Include(pf => pf.Products)
            .FirstOrDefaultAsync(pf => pf.Name == name && pf.IsActive);
    }

    public async Task<ProductFamily> CreateProductFamilyAsync(ProductFamily productFamily)
    {
        productFamily.CreatedAt = DateTime.UtcNow;
        productFamily.IsActive = true;
        _context.ProductFamilies.Add(productFamily);
        await _context.SaveChangesAsync();
        return productFamily;
    }

    public async Task<ProductFamily?> UpdateProductFamilyAsync(int id, ProductFamily productFamily)
    {
        var existingProductFamily = await _context.ProductFamilies.FindAsync(id);
        if (existingProductFamily == null)
            return null;

        existingProductFamily.Name = productFamily.Name;
        existingProductFamily.Description = productFamily.Description;
        existingProductFamily.Type = productFamily.Type;
        existingProductFamily.IsActive = productFamily.IsActive;
        existingProductFamily.ModifiedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingProductFamily;
    }

    public async Task<bool> DeleteProductFamilyAsync(int id)
    {
        var productFamily = await _context.ProductFamilies
            .Include(pf => pf.Products)
            .FirstOrDefaultAsync(pf => pf.Id == id);

        if (productFamily == null)
            return false;

        // Check if there are active products associated with this product family
        var hasActiveProducts = productFamily.Products.Any(p => p.IsActive);
        if (hasActiveProducts)
        {
            throw new InvalidOperationException("Cannot delete product family that has active products associated with it.");
        }

        // Soft delete by setting IsActive to false
        productFamily.IsActive = false;
        productFamily.ModifiedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ProductFamily>> GetProductFamiliesByTypeAsync(string type)
    {
        return await _context.ProductFamilies
            .Where(pf => pf.Type == type && pf.IsActive)
            .Include(pf => pf.Products)
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductFamily>> SearchProductFamiliesByNameAsync(string namePattern)
    {
        return await _context.ProductFamilies
            .Where(pf => pf.Name.Contains(namePattern) && pf.IsActive)
            .Include(pf => pf.Products)
            .ToListAsync();
    }
}
