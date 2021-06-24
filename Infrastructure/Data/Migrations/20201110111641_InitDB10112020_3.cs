using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDB10112020_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "MultipleChoicesExercise");

            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "EssayExercise");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "MultipleChoicesExercise",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "EssayExercise",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
