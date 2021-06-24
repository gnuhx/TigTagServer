using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class init_User_PasswordCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "testId",
                table: "Notification",
                newName: "TestId");

            migrationBuilder.AddColumn<string>(
                name: "PasswordCode",
                table: "User",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordCode",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "TestId",
                table: "Notification",
                newName: "testId");
        }
    }
}
