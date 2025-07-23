using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RimPoc.Data;

namespace RimPoc.Services;

public class DataSeederService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataSeederService> _logger;

    public DataSeederService(ApplicationDbContext context, ILogger<DataSeederService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting database seeding...");

            // Check if data already exists
            if (await _context.Countries.AnyAsync() || await _context.ControlledVocabularies.AnyAsync() || await _context.DefaultTemplates.AnyAsync())
            {
                _logger.LogInformation("Database already contains seed data. Skipping seeding.");
                return;
            }

            await SeedCountriesAsync();
            await SeedProductFamiliesAsync(); // This is synchronous but returns Task
            await SeedControlledVocabulariesAsync();
            await _context.SaveChangesAsync();
            await SeedDefaultTemplatesAsync();

            // Final save for any remaining changes
            await _context.SaveChangesAsync();
            _logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task SeedCountriesAsync()
    {
        var jsonPath = Path.Combine("SeedData", "Countries.json");
        if (!File.Exists(jsonPath))
        {
            _logger.LogWarning("Countries.json file not found at {Path}", jsonPath);
            return;
        }

        var jsonContent = await File.ReadAllTextAsync(jsonPath);
        var countries = JsonSerializer.Deserialize<List<CountryDto>>(jsonContent);

        if (countries == null || !countries.Any())
        {
            _logger.LogWarning("No countries found in seed data.");
            return;
        }

        foreach (var countryDto in countries)
        {
            var country = new Country
            {
                Name = countryDto.Name,
                Code = countryDto.Code,
                IsActive = countryDto.IsActive,
                CreatedAt = DateTime.SpecifyKind(countryDto.CreatedAt, DateTimeKind.Utc),
                ModifiedAt = countryDto.ModifiedAt.HasValue ? DateTime.SpecifyKind(countryDto.ModifiedAt.Value, DateTimeKind.Utc) : null
            };

            _context.Countries.Add(country);
        }

        _logger.LogInformation("Added {Count} countries to seeding queue.", countries.Count);
    }

    private Task SeedProductFamiliesAsync()
    {
        // Create default product families since the JSON file was deleted
        var productFamilies = new List<ProductFamily>
        {
            new ProductFamily
            {
                Name = "Infusion Systems",
                Description = "Devices used to deliver fluids, medications, or nutrients into a patient's body, such as infusion pumps and IV sets.",
                Type = "ProductFamily",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductFamily
            {
                Name = "Cardiac Rhythm Management Devices",
                Description = "Devices that monitor and regulate heart rhythm, including pacemakers and defibrillators.",
                Type = "ProductFamily",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductFamily
            {
                Name = "Oral Solid Dosage Forms (OSDF)",
                Description = "Pharmaceutical products in solid oral form such as tablets, capsules, and powders.",
                Type = "ProductFamily",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new ProductFamily
            {
                Name = "Injectable Biologics",
                Description = "Biological products administered via injection, including monoclonal antibodies and vaccines.",
                Type = "ProductFamily",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        _context.ProductFamilies.AddRange(productFamilies);
        _logger.LogInformation("Added {Count} product families to seeding queue.", productFamilies.Count);
        return Task.CompletedTask;
    }

    private async Task SeedControlledVocabulariesAsync()
    {
        var jsonPath = Path.Combine("SeedData", "ControlledVocabularies.json");
        if (!File.Exists(jsonPath))
        {
            _logger.LogWarning("ControlledVocabularies.json file not found at {Path}", jsonPath);
            return;
        }

        var jsonContent = await File.ReadAllTextAsync(jsonPath);
        var vocabularies = JsonSerializer.Deserialize<List<ControlledVocabularyDto>>(jsonContent);

        if (vocabularies == null || !vocabularies.Any())
        {
            _logger.LogWarning("No controlled vocabularies found in seed data.");
            return;
        }

        // Filter out ProductFamily entries that were incorrectly added to ControlledVocabularies
        var filteredVocabularies = vocabularies.Where(v => !string.IsNullOrEmpty(v.Code)).ToList();

        // First, insert parent records (those without ParentId)
        var parentRecords = filteredVocabularies.Where(v => v.ParentId == null).ToList();
        var childRecords = filteredVocabularies.Where(v => v.ParentId != null).ToList();

        foreach (var vocabDto in parentRecords)
        {
            var vocabulary = new ControlledVocabulary
            {
                Code = vocabDto.Code,
                Value = vocabDto.Value,
                Description = vocabDto.Description,
                Category = vocabDto.Category,
                Country = vocabDto.Country,
                IsActive = vocabDto.IsActive,
                DisplayOrder = vocabDto.DisplayOrder,
                CreatedAt = DateTime.SpecifyKind(vocabDto.CreatedAt, DateTimeKind.Utc),
                ModifiedAt = vocabDto.ModifiedAt.HasValue ? DateTime.SpecifyKind(vocabDto.ModifiedAt.Value, DateTimeKind.Utc) : null,
                ParentId = vocabDto.ParentId
            };

            _context.ControlledVocabularies.Add(vocabulary);
        }

        // Save parent records first
        await _context.SaveChangesAsync();

        // Now insert child records
        foreach (var vocabDto in childRecords)
        {
            var vocabulary = new ControlledVocabulary
            {
                Code = vocabDto.Code,
                Value = vocabDto.Value,
                Description = vocabDto.Description,
                Category = vocabDto.Category,
                Country = vocabDto.Country,
                IsActive = vocabDto.IsActive,
                DisplayOrder = vocabDto.DisplayOrder,
                CreatedAt = DateTime.SpecifyKind(vocabDto.CreatedAt, DateTimeKind.Utc),
                ModifiedAt = vocabDto.ModifiedAt.HasValue ? DateTime.SpecifyKind(vocabDto.ModifiedAt.Value, DateTimeKind.Utc) : null,
                ParentId = vocabDto.ParentId
            };

            _context.ControlledVocabularies.Add(vocabulary);
        }

        _logger.LogInformation("Added {ParentCount} parent and {ChildCount} child controlled vocabularies to seeding queue.",
            parentRecords.Count, childRecords.Count);
    }

    private async Task SeedDefaultTemplatesAsync()
    {
        // Check if DefaultTemplates already exist
        if (await _context.DefaultTemplates.AnyAsync())
        {
            _logger.LogInformation("DefaultTemplates already exist. Skipping seeding.");
            return;
        }

        var jsonPath = Path.Combine("SeedData", "DefaultTemplates.json");
        if (!File.Exists(jsonPath))
        {
            _logger.LogWarning("DefaultTemplates.json file not found at {Path}", jsonPath);
            return;
        }

        var jsonContent = await File.ReadAllTextAsync(jsonPath);
        var templateDtos = JsonSerializer.Deserialize<List<DefaultTemplatesDto>>(jsonContent);

        if (templateDtos == null || !templateDtos.Any())
        {
            _logger.LogWarning("No default templates found in seed data.");
            return;
        }

        var defaultTemplates = new List<DefaultTemplates>();

        foreach (var dto in templateDtos)
        {
            // Look up SubmissionTypeId by code in SubmissionActivity category
            var submissionType = await _context.ControlledVocabularies
                .FirstOrDefaultAsync(cv => cv.Code == dto.SubmissionType && cv.Category == "SubmissionActivity");
            if (submissionType == null)
            {
                _logger.LogWarning("SubmissionType with code '{SubmissionTypeCode}' not found for template '{TemplateName}'. Skipping.", dto.SubmissionType, dto.Name);
                continue;
            }

            var template = new DefaultTemplates
            {
                Name = dto.Name,
                SubmissionTypeId = submissionType.Id,
                Country = dto.Country,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            defaultTemplates.Add(template);
        }

        _context.DefaultTemplates.AddRange(defaultTemplates);
        _logger.LogInformation("Added {Count} default templates to seeding queue.", defaultTemplates.Count);
    }

    // DTOs for JSON deserialization
    private class CountryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }

    private class ControlledVocabularyDto
    {
        public string? Name { get; set; } // For ProductFamily entries (to filter out)
        public string Code { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Category { get; set; } = string.Empty;
        public string? Country { get; set; }
        public bool IsActive { get; set; }
        public int? DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public int? ParentId { get; set; }
    }

    private class DefaultTemplatesDto
    {
        public string Name { get; set; } = string.Empty;
        public string SubmissionType { get; set; } = string.Empty; // Code from ControlledVocabulary
        public string Country { get; set; } = string.Empty; // Code from Country
        public bool IsActive { get; set; }
    }
}