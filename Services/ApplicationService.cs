using Microsoft.EntityFrameworkCore;
using RimPoc.Data;

namespace RimPoc.Services;

public interface IApplicationService
{
    Task<Application?> GetByIdAsync(int id);
    Task<Application?> GetBySerialNumAsync(string serialNum);
    Task<Application?> GetByAppNumberAsync(string appNumber);
    Task<IEnumerable<Application>> GetAllAsync();
    Task<IEnumerable<Application>> GetAllActiveAsync();
    Task<IEnumerable<Application>> GetByCountryIdAsync(int countryId);
    Task<IEnumerable<Application>> GetByProductIdAsync(int productId);
    Task<IEnumerable<Application>> GetByStatusAsync(string status);
    Task<IEnumerable<Application>> GetByTypeAsync(string type);
    Task<IEnumerable<Application>> SearchByNameAsync(string namePattern);
    Task<IEnumerable<Application>> GetWithFilterAsync(bool? isActive = null, int skip = 0, int limit = 0);
    Task<Application> CreateAsync(Application application, IEnumerable<int> productIds);
    Task<Application> UpdateAsync(Application application, IEnumerable<int>? productIds = null);
    Task<bool> DeleteAsync(int id);
    Task<bool> AddProductToApplicationAsync(int applicationId, int productId);
    Task<bool> AddProductsToApplicationAsync(int applicationId, IEnumerable<int> productIds);
    Task<bool> RemoveProductFromApplicationAsync(int applicationId, int productId);
    Task<bool> RemoveAllProductsFromApplicationAsync(int applicationId);
    Task<IEnumerable<Product>> GetApplicationProductsAsync(int applicationId);
}

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _context;

    public ApplicationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Application?> GetByIdAsync(int id)
    {
        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.RiskVocabulary)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Application?> GetBySerialNumAsync(string serialNum)
    {
        if (string.IsNullOrWhiteSpace(serialNum))
            return null;

        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .FirstOrDefaultAsync(a => a.SerialNum == serialNum);
    }

    public async Task<Application?> GetByAppNumberAsync(string appNumber)
    {
        if (string.IsNullOrWhiteSpace(appNumber))
            return null;

        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .FirstOrDefaultAsync(a => a.AppNumber == appNumber);
    }

    public async Task<IEnumerable<Application>> GetAllAsync()
    {
        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetAllActiveAsync()
    {
        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .Where(a => a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetByCountryIdAsync(int countryId)
    {
        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .Where(a => a.CountryId == countryId && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetByProductIdAsync(int productId)
    {
        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .Where(a => a.Products.Any(p => p.Id == productId) && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetByStatusAsync(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return new List<Application>();

        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .Where(a => a.Status == status && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetByTypeAsync(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return new List<Application>();

        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .Where(a => a.Type == type && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> SearchByNameAsync(string namePattern)
    {
        if (string.IsNullOrWhiteSpace(namePattern))
            return new List<Application>();

        return await _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .Where(a => a.Name.Contains(namePattern) && a.IsActive)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Application>> GetWithFilterAsync(bool? isActive = null, int skip = 0, int limit = 0)
    {
        var query = _context.Applications
            .Include(a => a.Country)
            .Include(a => a.Products)
                .ThenInclude(p => p.ProductFamily)
            .AsQueryable();

        if (isActive.HasValue)
        {
            query = query.Where(a => a.IsActive == isActive.Value);
        }

        query = query.OrderBy(a => a.Name);

        if (skip > 0)
        {
            query = query.Skip(skip);
        }

        if (limit > 0)
        {
            query = query.Take(limit);
        }

        return await query.ToListAsync();
    }

    public async Task<Application> CreateAsync(Application application, IEnumerable<int> productIds)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        // Validate required fields
        if (string.IsNullOrWhiteSpace(application.SerialNum))
            throw new ArgumentException("SerialNum is required");

        if (string.IsNullOrWhiteSpace(application.Name))
            throw new ArgumentException("Name is required");

        // Check if SerialNum already exists
        var existingApplication = await _context.Applications
            .FirstOrDefaultAsync(a => a.SerialNum == application.SerialNum);
        if (existingApplication != null)
            throw new InvalidOperationException($"Application with SerialNum '{application.SerialNum}' already exists");

        // Validate CountryId exists
        var countryExists = await _context.Countries.AnyAsync(c => c.Id == application.CountryId);
        if (!countryExists)
            throw new ArgumentException($"Country with ID {application.CountryId} does not exist");

        // Validate RiskId if provided
        if (application.RiskId.HasValue)
        {
            var riskExists = await _context.ControlledVocabularies
                .AnyAsync(cv => cv.Id == application.RiskId.Value && cv.Category == "Risk");
            if (!riskExists)
                throw new ArgumentException($"Risk with ID {application.RiskId} does not exist");
        }

        // Validate that all product IDs exist
        if (productIds != null && productIds.Any())
        {
            var productIdsList = productIds.ToList();
            var existingProductIds = await _context.Products
                .Where(p => productIdsList.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            var missingProductIds = productIdsList.Except(existingProductIds).ToList();
            if (missingProductIds.Any())
                throw new ArgumentException($"Products with IDs {string.Join(", ", missingProductIds)} do not exist");

            // Load products to associate with the application
            var products = await _context.Products
                .Where(p => productIdsList.Contains(p.Id))
                .ToListAsync();

            application.Products = products;
        }

        // Set audit fields
        application.CreatedAt = DateTime.UtcNow;
        application.UpdatedAt = DateTime.UtcNow;
        application.IsActive = true;

        // Ensure StatusDate is UTC if provided
        if (application.StatusDate.HasValue && application.StatusDate.Value.Kind != DateTimeKind.Utc)
        {
            application.StatusDate = application.StatusDate.Value.ToUniversalTime();
        }

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();

        Console.WriteLine($"[DEBUG] Application created with ID: {application.Id} and {application.Products.Count} products");

        return await GetByIdAsync(application.Id) ?? application;
    }

    public async Task<Application> UpdateAsync(Application application, IEnumerable<int>? productIds = null)
    {
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        var existingApplication = await _context.Applications
            .Include(a => a.Products)
            .FirstOrDefaultAsync(a => a.Id == application.Id);

        if (existingApplication == null)
            throw new InvalidOperationException($"Application with ID {application.Id} not found");

        // Validate required fields
        if (string.IsNullOrWhiteSpace(application.SerialNum))
            throw new ArgumentException("SerialNum is required");

        if (string.IsNullOrWhiteSpace(application.Name))
            throw new ArgumentException("Name is required");

        // Check if SerialNum already exists for another application
        var duplicateSerialNum = await _context.Applications
            .FirstOrDefaultAsync(a => a.SerialNum == application.SerialNum && a.Id != application.Id);
        if (duplicateSerialNum != null)
            throw new InvalidOperationException($"Application with SerialNum '{application.SerialNum}' already exists");

        // Validate CountryId exists
        var countryExists = await _context.Countries.AnyAsync(c => c.Id == application.CountryId);
        if (!countryExists)
            throw new ArgumentException($"Country with ID {application.CountryId} does not exist");

        // Validate RiskId if provided
        if (application.RiskId.HasValue)
        {
            var riskExists = await _context.ControlledVocabularies
                .AnyAsync(cv => cv.Id == application.RiskId.Value && cv.Category == "Risk");
            if (!riskExists)
                throw new ArgumentException($"Risk with ID {application.RiskId} does not exist");
        }

        // Update products if provided
        if (productIds != null)
        {
            var productIdsList = productIds.ToList();
            if (productIdsList.Any())
            {
                // Validate that all product IDs exist
                var existingProductIds = await _context.Products
                    .Where(p => productIdsList.Contains(p.Id))
                    .Select(p => p.Id)
                    .ToListAsync();

                var missingProductIds = productIdsList.Except(existingProductIds).ToList();
                if (missingProductIds.Any())
                    throw new ArgumentException($"Products with IDs {string.Join(", ", missingProductIds)} do not exist");

                // Clear existing products and add new ones
                existingApplication.Products.Clear();
                var newProducts = await _context.Products
                    .Where(p => productIdsList.Contains(p.Id))
                    .ToListAsync();

                foreach (var product in newProducts)
                {
                    existingApplication.Products.Add(product);
                }
            }
            else
            {
                // Clear all products if empty list provided
                existingApplication.Products.Clear();
            }
        }

        // Update properties
        existingApplication.SerialNum = application.SerialNum;
        existingApplication.Name = application.Name;
        existingApplication.Type = application.Type;
        existingApplication.CountryId = application.CountryId;
        existingApplication.RiskId = application.RiskId;
        existingApplication.AppNumber = application.AppNumber;
        existingApplication.Status = application.Status;

        // Ensure StatusDate is UTC if provided
        if (application.StatusDate.HasValue && application.StatusDate.Value.Kind != DateTimeKind.Utc)
        {
            application.StatusDate = application.StatusDate.Value.ToUniversalTime();
        }
        existingApplication.StatusDate = application.StatusDate;

        existingApplication.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await GetByIdAsync(existingApplication.Id) ?? existingApplication;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var application = await _context.Applications.FindAsync(id);

        if (application == null)
            return false;

        _context.Applications.Remove(application);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddProductToApplicationAsync(int applicationId, int productId)
    {
        var application = await _context.Applications
            .Include(a => a.Products)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (application == null)
            return false;

        // Validate that the product ID exists
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
            throw new ArgumentException($"Product with ID {productId} does not exist");

        // Check if product is already associated
        if (application.Products.Any(p => p.Id == productId))
            return true; // Already associated

        application.Products.Add(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddProductsToApplicationAsync(int applicationId, IEnumerable<int> productIds)
    {
        var application = await _context.Applications
            .Include(a => a.Products)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (application == null)
            return false;

        var productIdsList = productIds.ToList();
        if (!productIdsList.Any())
            return true;

        // Validate that all product IDs exist
        var existingProductIds = await _context.Products
            .Where(p => productIdsList.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync();

        var missingProductIds = productIdsList.Except(existingProductIds).ToList();
        if (missingProductIds.Any())
            throw new ArgumentException($"Products with IDs {string.Join(", ", missingProductIds)} do not exist");

        // Get products to add (exclude already associated ones)
        var currentProductIds = application.Products.Select(p => p.Id).ToList();
        var productsToAdd = await _context.Products
            .Where(p => productIdsList.Contains(p.Id) && !currentProductIds.Contains(p.Id))
            .ToListAsync();

        foreach (var product in productsToAdd)
        {
            application.Products.Add(product);
        }

        if (productsToAdd.Any())
        {
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<bool> RemoveProductFromApplicationAsync(int applicationId, int productId)
    {
        var application = await _context.Applications
            .Include(a => a.Products)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (application == null)
            return false;

        var productToRemove = application.Products.FirstOrDefault(p => p.Id == productId);
        if (productToRemove == null)
            return true; // Not associated

        application.Products.Remove(productToRemove);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveAllProductsFromApplicationAsync(int applicationId)
    {
        var application = await _context.Applications
            .Include(a => a.Products)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (application == null)
            return false;

        application.Products.Clear();
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Product>> GetApplicationProductsAsync(int applicationId)
    {
        return await _context.Applications
            .Where(a => a.Id == applicationId)
            .SelectMany(a => a.Products)
            .Include(p => p.ProductFamily)
            .ToListAsync();
    }
}