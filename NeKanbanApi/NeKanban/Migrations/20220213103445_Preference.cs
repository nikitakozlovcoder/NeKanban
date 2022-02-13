using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Migrations
{
    public partial class Preference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Preference",
                table: "DeskUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preference",
                table: "DeskUser");
        }
    }
}
