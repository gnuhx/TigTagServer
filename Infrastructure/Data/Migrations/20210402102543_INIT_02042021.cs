using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class INIT_02042021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "EssayAnswer");

            migrationBuilder.DropTable(
                name: "EssayExercise");

            migrationBuilder.DropTable(
                name: "MultipleChoicesAnswer");

            migrationBuilder.DropTable(
                name: "MultipleChoicesExercise");

            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Test");

            migrationBuilder.DropColumn(
                name: "IsCompetitor",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Notification");

            migrationBuilder.AddColumn<int>(
                name: "AccessCount",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TigtagUrl",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tiktok",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserLoginName",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Youtube",
                table: "User",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Relation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    LinkToId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Relation_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Relation_UserId",
                table: "Relation",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relation");

            migrationBuilder.DropColumn(
                name: "AccessCount",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "User");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TigtagUrl",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Tiktok",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserLoginName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Youtube",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompetitor",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "Notification",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsReplied = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contact_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Utility = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFixed = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Answer_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EssayExercise",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EssayType = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FalseResult1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FalseResult2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FalseResult3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    RightResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "EssayAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EssayExerciseId = table.Column<int>(type: "int", nullable: false),
                    IsBingo = table.Column<bool>(type: "bit", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultFixed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<double>(type: "float", nullable: false),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerId = table.Column<int>(type: "int", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsBingo = table.Column<bool>(type: "bit", nullable: false),
                    MultipleChoicesExerciseId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "IX_Answer_TestId",
                table: "Answer",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contact_UserId",
                table: "Contact",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EssayAnswer_AnswerId",
                table: "EssayAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_EssayExercise_TestId",
                table: "EssayExercise",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoicesAnswer_AnswerId",
                table: "MultipleChoicesAnswer",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoicesExercise_TestId",
                table: "MultipleChoicesExercise",
                column: "TestId");
        }
    }
}
