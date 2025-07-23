using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionToC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubmissionToCs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Parent = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Section = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    LeafTitle = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Href = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SubmissionId = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedDays = table.Column<int>(type: "integer", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissionToCs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmissionToCs_Submissions_SubmissionId",
                        column: x => x.SubmissionId,
                        principalTable: "Submissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionToCs_Parent",
                table: "SubmissionToCs",
                column: "Parent");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionToCs_Section",
                table: "SubmissionToCs",
                column: "Section");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissionToCs_SubmissionId",
                table: "SubmissionToCs",
                column: "SubmissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissionToCs");
        }
    }
}
