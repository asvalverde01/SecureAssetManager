using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureAssetManager.Migrations
{
    public partial class Ass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Valoracion",
                table: "Assets",
                newName: "ValoracionIntegridad");

            migrationBuilder.AddColumn<int>(
                name: "ValoracionConfidencialidad",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValoracionDisponibilidad",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValoracionConfidencialidad",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "ValoracionDisponibilidad",
                table: "Assets");

            migrationBuilder.RenameColumn(
                name: "ValoracionIntegridad",
                table: "Assets",
                newName: "Valoracion");
        }
    }
}
