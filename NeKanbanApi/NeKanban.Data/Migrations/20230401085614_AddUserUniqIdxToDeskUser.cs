using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserUniqIdxToDeskUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeskUser_DeskId",
                table: "DeskUser");

            migrationBuilder.CreateIndex(
                name: "IX_DeskUser_DeskId_UserId",
                table: "DeskUser",
                columns: new[] { "DeskId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DeskUser_DeskId_UserId",
                table: "DeskUser");

            migrationBuilder.CreateIndex(
                name: "IX_DeskUser_DeskId",
                table: "DeskUser",
                column: "DeskId");
        }
    }
}
