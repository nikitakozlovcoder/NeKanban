using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeToToDo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "ToDo",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ToDo_Id_Code",
                table: "ToDo",
                columns: new[] { "Id", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ToDo_Id_Code",
                table: "ToDo");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ToDo");
        }
    }
}
