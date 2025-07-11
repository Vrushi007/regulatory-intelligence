using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class MoveRiskFromProductToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ControlledVocabularies_RiskId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_RiskId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RiskId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "RiskId",
                table: "Applications",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RiskId",
                table: "Applications",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_ControlledVocabularies_RiskId",
                table: "Applications",
                column: "RiskId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_ControlledVocabularies_RiskId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_RiskId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "RiskId",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "RiskId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_RiskId",
                table: "Products",
                column: "RiskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ControlledVocabularies_RiskId",
                table: "Products",
                column: "RiskId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
