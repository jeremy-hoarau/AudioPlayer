using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AudioPlayer.Data.Migrations
{
    public partial class Songs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "Songs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Songs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Songs");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Songs");
        }
    }
}
