using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Data.Migrations
{
    public partial class CommentBehaviourOnDeskUserDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_DeskUser_DeskUserId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_DeskUser_DeskUserId",
                table: "Comments",
                column: "DeskUserId",
                principalTable: "DeskUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_DeskUser_DeskUserId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_DeskUser_DeskUserId",
                table: "Comments",
                column: "DeskUserId",
                principalTable: "DeskUser",
                principalColumn: "Id");
        }
    }
}
