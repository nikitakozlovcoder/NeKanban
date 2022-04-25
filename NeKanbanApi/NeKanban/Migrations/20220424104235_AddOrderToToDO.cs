using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Migrations
{
    public partial class AddOrderToToDO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ToDo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ToDo");
        }
    }
}
