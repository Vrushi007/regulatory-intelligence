using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanAndPlanDocumentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "SubmissionToCs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedDays",
                table: "SubmissionToCs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "SubmissionToCs",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "SubmissionToCs");

            migrationBuilder.DropColumn(
                name: "EstimatedDays",
                table: "SubmissionToCs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "SubmissionToCs");
        }
    }
}
