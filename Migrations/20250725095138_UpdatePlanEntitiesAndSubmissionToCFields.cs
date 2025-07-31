using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlanEntitiesAndSubmissionToCFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedDays = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDocuments_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanDocumentSubmissionToCMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlanDocumentId = table.Column<int>(type: "integer", nullable: false),
                    SubmissionToCId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDocumentSubmissionToCMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDocumentSubmissionToCMaps_PlanDocuments_PlanDocumentId",
                        column: x => x.PlanDocumentId,
                        principalTable: "PlanDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanDocumentSubmissionToCMaps_SubmissionToCs_SubmissionToCId",
                        column: x => x.SubmissionToCId,
                        principalTable: "SubmissionToCs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanDocuments_PlanId",
                table: "PlanDocuments",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDocumentSubmissionToCMaps_PlanDocumentId",
                table: "PlanDocumentSubmissionToCMaps",
                column: "PlanDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanDocumentSubmissionToCMaps_SubmissionToCId",
                table: "PlanDocumentSubmissionToCMaps",
                column: "SubmissionToCId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanDocumentSubmissionToCMaps");

            migrationBuilder.DropTable(
                name: "PlanDocuments");

            migrationBuilder.DropTable(
                name: "Plans");
        }
    }
}
