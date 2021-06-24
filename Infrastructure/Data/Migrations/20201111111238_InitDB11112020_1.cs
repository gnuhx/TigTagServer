using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDB11112020_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Student",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Student");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Student",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
