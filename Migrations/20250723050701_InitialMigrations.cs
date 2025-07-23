using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControlledVocabularies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ParentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlledVocabularies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlledVocabularies_ControlledVocabularies_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductFamilies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFamilies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DefaultTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SubmissionTypeId = table.Column<int>(type: "integer", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultTemplates_ControlledVocabularies_SubmissionTypeId",
                        column: x => x.SubmissionTypeId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNum = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    RiskId = table.Column<int>(type: "integer", nullable: true),
                    AppNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_ControlledVocabularies_RiskId",
                        column: x => x.RiskId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Applications_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CodeName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ProductFamilyId = table.Column<int>(type: "integer", nullable: false),
                    MedicalSpecialtyId = table.Column<int>(type: "integer", nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: true),
                    SubtypeId = table.Column<int>(type: "integer", nullable: true),
                    FunctionsId = table.Column<int>(type: "integer", nullable: true),
                    EnergySourceId = table.Column<int>(type: "integer", nullable: true),
                    RadiationTypeId = table.Column<int>(type: "integer", nullable: true),
                    RadiationEmitting = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ControlledVocabularies_EnergySourceId",
                        column: x => x.EnergySourceId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ControlledVocabularies_FunctionsId",
                        column: x => x.FunctionsId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ControlledVocabularies_MedicalSpecialtyId",
                        column: x => x.MedicalSpecialtyId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ControlledVocabularies_RadiationTypeId",
                        column: x => x.RadiationTypeId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ControlledVocabularies_SubtypeId",
                        column: x => x.SubtypeId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ControlledVocabularies_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Products_ProductFamilies_ProductFamilyId",
                        column: x => x.ProductFamilyId,
                        principalTable: "ProductFamilies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    SequenceNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SubmissionActivityId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SubmissionNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Submissions_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Submissions_ControlledVocabularies_SubmissionActivityId",
                        column: x => x.SubmissionActivityId,
                        principalTable: "ControlledVocabularies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationProducts",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationProducts", x => new { x.ApplicationId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ApplicationProducts_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationProducts_ProductId",
                table: "ApplicationProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AppNumber",
                table: "Applications",
                column: "AppNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_CountryId",
                table: "Applications",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RiskId",
                table: "Applications",
                column: "RiskId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_SerialNum",
                table: "Applications",
                column: "SerialNum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlledVocabularies_Category",
                table: "ControlledVocabularies",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_ControlledVocabularies_Category_Code",
                table: "ControlledVocabularies",
                columns: new[] { "Category", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlledVocabularies_ParentId",
                table: "ControlledVocabularies",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Code",
                table: "Countries",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTemplates_Country_SubmissionTypeId_Name",
                table: "DefaultTemplates",
                columns: new[] { "Country", "SubmissionTypeId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTemplates_IsActive",
                table: "DefaultTemplates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTemplates_SubmissionTypeId",
                table: "DefaultTemplates",
                column: "SubmissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_EnergySourceId",
                table: "Products",
                column: "EnergySourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_FunctionsId",
                table: "Products",
                column: "FunctionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MedicalSpecialtyId",
                table: "Products",
                column: "MedicalSpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductFamilyId",
                table: "Products",
                column: "ProductFamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RadiationTypeId",
                table: "Products",
                column: "RadiationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubtypeId",
                table: "Products",
                column: "SubtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_TypeId",
                table: "Products",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_ApplicationId_SequenceNumber",
                table: "Submissions",
                columns: new[] { "ApplicationId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_Status",
                table: "Submissions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SubmissionActivityId",
                table: "Submissions",
                column: "SubmissionActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_SubmissionNumber",
                table: "Submissions",
                column: "SubmissionNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationProducts");

            migrationBuilder.DropTable(
                name: "DefaultTemplates");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "ProductFamilies");

            migrationBuilder.DropTable(
                name: "ControlledVocabularies");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
