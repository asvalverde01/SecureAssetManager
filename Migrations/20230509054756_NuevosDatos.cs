using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureAssetManager.Migrations
{
    public partial class NuevosDatos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Responsable",
                table: "Assets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "Assets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsable",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "Ubicacion",
                table: "Assets");
        }
    }
}
