using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDB17122020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewWords",
                table: "Test");

            migrationBuilder.AddColumn<string>(
                name: "Utility",
                table: "Test",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Utility",
                table: "Test");

            migrationBuilder.AddColumn<string>(
                name: "NewWords",
                table: "Test",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
