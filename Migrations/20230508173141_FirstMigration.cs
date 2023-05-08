using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecureAssetManager.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AppUser_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    TempId1 = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("AK_AppUser_TempId1", x => x.TempId1);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AppUser_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
