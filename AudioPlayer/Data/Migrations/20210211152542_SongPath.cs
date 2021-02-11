using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AudioPlayer.Data.Migrations
{
    public partial class SongPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "File",
                table: "Songs");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Songs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Songs");

            migrationBuilder.AddColumn<byte[]>(
                name: "File",
                table: "Songs",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
