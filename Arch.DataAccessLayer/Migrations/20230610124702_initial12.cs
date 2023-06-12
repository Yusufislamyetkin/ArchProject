using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arch.DataAccessLayer.Migrations
{
    public partial class initial12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilePath_Competitions_CompetitionId",
                table: "ProjectFilePath");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectFilePath",
                table: "ProjectFilePath");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Competitions");

            migrationBuilder.RenameTable(
                name: "ProjectFilePath",
                newName: "ProjectFilePaths");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectFilePath_CompetitionId",
                table: "ProjectFilePaths",
                newName: "IX_ProjectFilePaths_CompetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectFilePaths",
                table: "ProjectFilePaths",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilePaths_Competitions_CompetitionId",
                table: "ProjectFilePaths",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilePaths_Competitions_CompetitionId",
                table: "ProjectFilePaths");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectFilePaths",
                table: "ProjectFilePaths");

            migrationBuilder.RenameTable(
                name: "ProjectFilePaths",
                newName: "ProjectFilePath");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectFilePaths_CompetitionId",
                table: "ProjectFilePath",
                newName: "IX_ProjectFilePath_CompetitionId");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Competitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectFilePath",
                table: "ProjectFilePath",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilePath_Competitions_CompetitionId",
                table: "ProjectFilePath",
                column: "CompetitionId",
                principalTable: "Competitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
