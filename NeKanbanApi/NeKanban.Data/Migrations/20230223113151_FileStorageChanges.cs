using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NeKanban.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileStorageChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoFileAdapter_FileEntity_FileId",
                table: "ToDoFileAdapter");

            migrationBuilder.DropTable(
                name: "FileEntity");

            migrationBuilder.CreateTable(
                name: "FileStorageEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileStorageEntity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoFileAdapter_FileStorageEntity_FileId",
                table: "ToDoFileAdapter",
                column: "FileId",
                principalTable: "FileStorageEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoFileAdapter_FileStorageEntity_FileId",
                table: "ToDoFileAdapter");

            migrationBuilder.DropTable(
                name: "FileStorageEntity");

            migrationBuilder.CreateTable(
                name: "FileEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntity", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoFileAdapter_FileEntity_FileId",
                table: "ToDoFileAdapter",
                column: "FileId",
                principalTable: "FileEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
