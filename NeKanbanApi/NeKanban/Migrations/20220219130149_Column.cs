using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Migrations
{
    public partial class Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Column",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Column",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Column");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Column");
        }
    }
}
