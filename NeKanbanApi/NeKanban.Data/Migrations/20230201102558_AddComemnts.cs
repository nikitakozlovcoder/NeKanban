using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NeKanban.Data.Migrations
{
    public partial class AddComemnts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeskUserId = table.Column<int>(type: "integer", nullable: true),
                    ToDoId = table.Column<int>(type: "integer", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_DeskUser_DeskUserId",
                        column: x => x.DeskUserId,
                        principalTable: "DeskUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_ToDo_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_DeskUserId",
                table: "Comments",
                column: "DeskUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ToDoId",
                table: "Comments",
                column: "ToDoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
