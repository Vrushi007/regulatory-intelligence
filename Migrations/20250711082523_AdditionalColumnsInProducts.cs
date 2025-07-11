using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalColumnsInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ControlledVocabularies_ClassificationId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ClassificationId",
                table: "Products",
                newName: "RadiationTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ClassificationId",
                table: "Products",
                newName: "IX_Products_RadiationTypeId");

            migrationBuilder.AddColumn<int>(
                name: "EnergySourceId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FunctionsId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MedicalSpecialtyId",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RadiationEmitting",
                table: "Products",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ControlledVocabularies_EnergySourceId",
                table: "Products",
                column: "EnergySourceId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ControlledVocabularies_FunctionsId",
                table: "Products",
                column: "FunctionsId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ControlledVocabularies_MedicalSpecialtyId",
                table: "Products",
                column: "MedicalSpecialtyId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ControlledVocabularies_RadiationTypeId",
                table: "Products",
                column: "RadiationTypeId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ControlledVocabularies_EnergySourceId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ControlledVocabularies_FunctionsId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ControlledVocabularies_MedicalSpecialtyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ControlledVocabularies_RadiationTypeId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_EnergySourceId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_FunctionsId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_MedicalSpecialtyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EnergySourceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "FunctionsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MedicalSpecialtyId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RadiationEmitting",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "RadiationTypeId",
                table: "Products",
                newName: "ClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_RadiationTypeId",
                table: "Products",
                newName: "IX_Products_ClassificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ControlledVocabularies_ClassificationId",
                table: "Products",
                column: "ClassificationId",
                principalTable: "ControlledVocabularies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
