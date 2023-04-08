using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDeletionReasonToDeskUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DeskUser");

            migrationBuilder.AddColumn<int>(
                name: "DeletionReason",
                table: "DeskUser",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "DeskUser");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DeskUser",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
