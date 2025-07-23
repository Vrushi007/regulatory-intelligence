using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rim_poc.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultTemplateContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultTemplateContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Parent = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Section = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    LeafTitle = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Href = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultTemplateContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultTemplateContents_DefaultTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "DefaultTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTemplateContents_Parent",
                table: "DefaultTemplateContents",
                column: "Parent");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTemplateContents_Section",
                table: "DefaultTemplateContents",
                column: "Section");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultTemplateContents_TemplateId",
                table: "DefaultTemplateContents",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultTemplateContents");
        }
    }
}
