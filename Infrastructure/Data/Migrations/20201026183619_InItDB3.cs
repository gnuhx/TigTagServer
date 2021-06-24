using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InItDB3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Annotation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    IsChecked = table.Column<bool>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Annotation_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    TestId = table.Column<int>(nullable: false),
                    MultipleChoicesAnswers = table.Column<string>(nullable: true),
                    EssayAnswers = table.Column<string>(nullable: true),
                    Score = table.Column<double>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EssayExercise",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    TestId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EssayExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EssayExercise_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoicesExercise",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    TestId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    RightResult = table.Column<string>(nullable: true),
                    FalseResult1 = table.Column<string>(nullable: true),
                    FalseResult2 = table.Column<string>(nullable: true),
                    FalseResult3 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoicesExercise", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoicesExercise_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annotation_StudentId",
                table: "Annotation",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_StudentId",
                table: "Answer",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_TestId",
                table: "Answer",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_EssayExercise_TestId",
                table: "EssayExercise",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoicesExercise_TestId",
                table: "MultipleChoicesExercise",
                column: "TestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annotation");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "EssayExercise");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "MultipleChoicesExercise");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}
