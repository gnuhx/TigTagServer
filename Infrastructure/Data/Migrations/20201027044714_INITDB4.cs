using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class INITDB4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EssayAnswers",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "MultipleChoicesAnswers",
                table: "Answer");

            migrationBuilder.CreateTable(
                name: "EssayAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    EssayExerciseId = table.Column<int>(nullable: false),
                    AnswerId = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EssayAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EssayAnswer_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoicesAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    EssayExerciseId = table.Column<int>(nullable: false),
                    AnswerId = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoicesAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoicesAnswer_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EssayAnswer_AnswerId",
                table: "EssayAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoicesAnswer_AnswerId",
                table: "MultipleChoicesAnswer",
                column: "AnswerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EssayAnswer");

            migrationBuilder.DropTable(
                name: "MultipleChoicesAnswer");

            migrationBuilder.AddColumn<string>(
                name: "EssayAnswers",
                table: "Answer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MultipleChoicesAnswers",
                table: "Answer",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
