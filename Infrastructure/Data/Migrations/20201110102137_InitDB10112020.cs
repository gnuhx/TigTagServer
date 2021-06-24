using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data.Migrations
{
    public partial class InitDB10112020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "Test",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "testId",
                table: "Annotation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "Test");

            migrationBuilder.DropColumn(
                name: "testId",
                table: "Annotation");
        }
    }
}
