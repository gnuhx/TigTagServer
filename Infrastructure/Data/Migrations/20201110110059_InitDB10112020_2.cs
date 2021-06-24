using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDB10112020_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "Test");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "MultipleChoicesExercise",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "MultipleChoicesExercise",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "EssayExercise",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "EssayExercise",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "MultipleChoicesExercise");

            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "MultipleChoicesExercise");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "EssayExercise");

            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "EssayExercise");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Test",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "Test",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
