using Microsoft.EntityFrameworkCore.Migrations;

namespace TechNationAPI.Migrations
{
    public partial class Agoralog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgoraLog",
                table: "LogsTechNation",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgoraLog",
                table: "LogsTechNation");
        }
    }
}
