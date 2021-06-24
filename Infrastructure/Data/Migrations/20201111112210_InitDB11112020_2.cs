using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDB11112020_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annotation_Student_StudentId",
                table: "Annotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Student_StudentId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_StudentId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Annotation_StudentId",
                table: "Annotation");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Annotation");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Answer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Annotation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_UserId",
                table: "Answer",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Annotation_UserId",
                table: "Annotation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Annotation_User_UserId",
                table: "Annotation",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_User_UserId",
                table: "Answer",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Annotation_User_UserId",
                table: "Annotation");

            migrationBuilder.DropForeignKey(
                name: "FK_Answer_User_UserId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Answer_UserId",
                table: "Answer");

            migrationBuilder.DropIndex(
                name: "IX_Annotation_UserId",
                table: "Annotation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Answer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Annotation");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Answer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Annotation",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Answer_StudentId",
                table: "Answer",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Annotation_StudentId",
                table: "Annotation",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Annotation_Student_StudentId",
                table: "Annotation",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Student_StudentId",
                table: "Answer",
                column: "StudentId",
                principalTable: "Student",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
