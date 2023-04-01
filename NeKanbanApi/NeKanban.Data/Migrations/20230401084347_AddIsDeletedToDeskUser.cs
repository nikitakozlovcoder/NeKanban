using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToDeskUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DeskUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeskUser");
        }
    }
}
