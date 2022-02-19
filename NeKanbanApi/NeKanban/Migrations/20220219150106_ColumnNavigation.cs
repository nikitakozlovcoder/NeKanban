using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Migrations
{
    public partial class ColumnNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeskId",
                table: "Column",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Column_DeskId",
                table: "Column",
                column: "DeskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Column_Desk_DeskId",
                table: "Column",
                column: "DeskId",
                principalTable: "Desk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Column_Desk_DeskId",
                table: "Column");

            migrationBuilder.DropIndex(
                name: "IX_Column_DeskId",
                table: "Column");

            migrationBuilder.DropColumn(
                name: "DeskId",
                table: "Column");
        }
    }
}
