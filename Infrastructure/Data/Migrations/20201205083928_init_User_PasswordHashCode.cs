using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class init_User_PasswordHashCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordCode",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHashCode",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHashCode",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "PasswordCode",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
