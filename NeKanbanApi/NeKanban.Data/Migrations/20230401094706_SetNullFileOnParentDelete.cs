using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NeKanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetNullFileOnParentDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentFileAdapter_Comments_ParentId",
                table: "CommentFileAdapter");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoFileAdapter_ToDo_ParentId",
                table: "ToDoFileAdapter");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFileAdapter_Comments_ParentId",
                table: "CommentFileAdapter",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoFileAdapter_ToDo_ParentId",
                table: "ToDoFileAdapter",
                column: "ParentId",
                principalTable: "ToDo",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentFileAdapter_Comments_ParentId",
                table: "CommentFileAdapter");

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoFileAdapter_ToDo_ParentId",
                table: "ToDoFileAdapter");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentFileAdapter_Comments_ParentId",
                table: "CommentFileAdapter",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoFileAdapter_ToDo_ParentId",
                table: "ToDoFileAdapter",
                column: "ParentId",
                principalTable: "ToDo",
                principalColumn: "Id");
        }
    }
}
