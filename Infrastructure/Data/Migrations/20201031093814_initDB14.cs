using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class initDB14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultipleChoicesId",
                table: "MultipleChoicesAnswer");

            migrationBuilder.AddColumn<int>(
                name: "MultipleChoicesExerciseId",
                table: "MultipleChoicesAnswer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultipleChoicesExerciseId",
                table: "MultipleChoicesAnswer");

            migrationBuilder.AddColumn<int>(
                name: "MultipleChoicesId",
                table: "MultipleChoicesAnswer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
