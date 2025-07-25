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
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<ControlledVocabulary> ControlledVocabularies { get; set; }
    public DbSet<DefaultTemplates> DefaultTemplates { get; set; }
    public DbSet<DefaultTemplateContent> DefaultTemplateContents { get; set; }
    public DbSet<SubmissionToC> SubmissionToCs { get; set; }

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

        // Configure Submission entity
        modelBuilder.Entity<Submission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SequenceNumber)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
            entity.Property(e => e.SubmissionNumber)
                .HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50);

            // Configure relationship with Application
            entity.HasOne(e => e.Application)
                .WithMany(a => a.Submissions)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Configure relationship with ControlledVocabulary for Submission Activity
            entity.HasOne(e => e.SubmissionActivity)
                .WithMany() // No back navigation needed
                .HasForeignKey(e => e.SubmissionActivityId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Indexes for better performance
            entity.HasIndex(e => new { e.ApplicationId, e.SequenceNumber })
                .IsUnique(); // Unique sequence number per application
            entity.HasIndex(e => e.SubmissionNumber);
            entity.HasIndex(e => e.Status);
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
            entity.Property(e => e.Country); // No length restriction for comma-separated values

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

        // Configure DefaultTemplates entity
        modelBuilder.Entity<DefaultTemplates>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Country)
                .IsRequired();

            // Configure relationship with ControlledVocabulary for SubmissionType
            entity.HasOne(e => e.SubmissionType)
                .WithMany() // No back navigation needed
                .HasForeignKey(e => e.SubmissionTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Indexes for better performance
            entity.HasIndex(e => new { e.Country, e.SubmissionTypeId, e.Name })
                .IsUnique(); // Unique template per country, submission type, and name
            entity.HasIndex(e => e.IsActive);
        });

        // Configure DefaultTemplateContent entity
        modelBuilder.Entity<DefaultTemplateContent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Parent)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.Section)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.LeafTitle)
                .HasMaxLength(300);
            entity.Property(e => e.FileName)
                .HasMaxLength(255);
            entity.Property(e => e.Href)
                .HasMaxLength(1000);

            // Configure relationship with DefaultTemplates
            entity.HasOne(e => e.Template)
                .WithMany() // No back navigation needed
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Indexes for better performance
            entity.HasIndex(e => e.TemplateId);
            entity.HasIndex(e => e.Parent);
            entity.HasIndex(e => e.Section);
        });

        // Configure SubmissionToC entity
        modelBuilder.Entity<SubmissionToC>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Parent)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.Section)
                .IsRequired()
                .HasMaxLength(300);
            entity.Property(e => e.LeafTitle)
                .HasMaxLength(300);
            entity.Property(e => e.FileName)
                .HasMaxLength(255);
            entity.Property(e => e.Href)
                .HasMaxLength(1000);

            // Configure relationship with Submission
            entity.HasOne(e => e.Submission)
                .WithMany() // No back navigation needed
                .HasForeignKey(e => e.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            // Indexes for better performance
            entity.HasIndex(e => e.SubmissionId);
            entity.HasIndex(e => e.Parent);
            entity.HasIndex(e => e.Section);
        });
    }
}