using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureAssetManager.Migrations
{
    public partial class control : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "Risks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Controls",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoControl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Efectividad = table.Column<int>(type: "int", nullable: false),
                    BrechaSeguridadIdentificada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Implementacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Monitoreo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Controls", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Controls");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "Risks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
