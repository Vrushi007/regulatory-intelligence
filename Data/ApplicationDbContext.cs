using Microsoft.EntityFrameworkCore;

namespace RimPoc.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<ProductFamily> ProductFamilies { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<ControlledVocabulary> ControlledVocabularies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Country entity
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(3);

            // Indexes
            entity.HasIndex(e => e.Code)
                .IsUnique();
        });

        // Configure ProductFamily entity
        modelBuilder.Entity<ProductFamily>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            entity.Property(e => e.Type)
                .HasMaxLength(50);
        });        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.CodeName)
                .HasMaxLength(50);
            entity.Property(e => e.Description)
                .HasMaxLength(500);

            // Configure relationship with ProductFamily
            entity.HasOne(e => e.ProductFamily)
                .WithMany(pf => pf.Products)
                .HasForeignKey(e => e.ProductFamilyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships with ControlledVocabulary
            entity.HasOne(e => e.MedicalSpecialty)
                .WithMany(cv => cv.ProductsAsMedicalSpecialty)
                .HasForeignKey(e => e.MedicalSpecialtyId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.ProductType)
                .WithMany(cv => cv.ProductsAsType)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.ProductSubtype)
                .WithMany(cv => cv.ProductsAsSubtype)
                .HasForeignKey(e => e.SubtypeId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Functions)
                .WithMany(cv => cv.ProductsAsFunctions)
                .HasForeignKey(e => e.FunctionsId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.EnergySource)
                .WithMany(cv => cv.ProductsAsEnergySource)
                .HasForeignKey(e => e.EnergySourceId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.RadiationType)
                .WithMany(cv => cv.ProductsAsRadiationType)
                .HasForeignKey(e => e.RadiationTypeId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configure Application entity
        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SerialNum)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Type)
                .HasMaxLength(50);
            entity.Property(e => e.AppNumber)
                .HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50);

            // Configure relationship with Country
            entity.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Configure relationship with ControlledVocabulary for Risk
            entity.HasOne(e => e.RiskVocabulary)
                .WithMany(cv => cv.ApplicationsAsRisk)
                .HasForeignKey(e => e.RiskId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            entity.HasIndex(e => e.SerialNum)
                .IsUnique();
            entity.HasIndex(e => e.AppNumber);
        });

        // Configure many-to-many relationship between Application and Product
        modelBuilder.Entity<Application>()
            .HasMany(a => a.Products)
            .WithMany(p => p.Applications)
            .UsingEntity<Dictionary<string, object>>(
                "ApplicationProduct",
                j => j
                    .HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Application>()
                    .WithMany()
                    .HasForeignKey("ApplicationId")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("ApplicationId", "ProductId");
                    j.ToTable("ApplicationProducts");
                });

        // Configure ControlledVocabulary entity
        modelBuilder.Entity<ControlledVocabulary>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);

            // Self-referencing relationship for Parent-Child (e.g., ProductType -> ProductSubtypes)
            entity.HasOne(e => e.Parent)
                .WithMany(e => e.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes for better performance
            entity.HasIndex(e => new { e.Category, e.Code })
                .IsUnique();
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.ParentId);
        });
    }
}