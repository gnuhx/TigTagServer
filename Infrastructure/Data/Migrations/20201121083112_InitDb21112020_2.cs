using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDb21112020_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFixed",
                table: "Answer");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsFixed",
                table: "Answer",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
