using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechNationAPI.Migrations
{
    public partial class Addedproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "LogsTechNation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MinhaCdnLog",
                table: "LogsTechNation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "LogsTechNation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "LogsTechNation");

            migrationBuilder.DropColumn(
                name: "MinhaCdnLog",
                table: "LogsTechNation");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "LogsTechNation");
        }
    }
}
