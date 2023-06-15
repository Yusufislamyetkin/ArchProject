using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arch.DataAccessLayer.Migrations
{
    public partial class initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilePaths_DesignerUsers_DesignerUserId1",
                table: "ProjectFilePaths");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFilePaths_DesignerUserId1",
                table: "ProjectFilePaths");

            migrationBuilder.DropColumn(
                name: "DesignerUserId",
                table: "ProjectFilePaths");

            migrationBuilder.DropColumn(
                name: "DesignerUserId1",
                table: "ProjectFilePaths");

            migrationBuilder.DropColumn(
                name: "ProjectFilePathID",
                table: "DesignerUsers");

            migrationBuilder.AddColumn<string>(
                name: "DesignerId",
                table: "ProjectFilePaths",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFilePaths_DesignerId",
                table: "ProjectFilePaths",
                column: "DesignerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilePaths_AspNetUsers_DesignerId",
                table: "ProjectFilePaths",
                column: "DesignerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFilePaths_AspNetUsers_DesignerId",
                table: "ProjectFilePaths");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFilePaths_DesignerId",
                table: "ProjectFilePaths");

            migrationBuilder.DropColumn(
                name: "DesignerId",
                table: "ProjectFilePaths");

            migrationBuilder.AddColumn<string>(
                name: "DesignerUserId",
                table: "ProjectFilePaths",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignerUserId1",
                table: "ProjectFilePaths",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectFilePathID",
                table: "DesignerUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFilePaths_DesignerUserId1",
                table: "ProjectFilePaths",
                column: "DesignerUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFilePaths_DesignerUsers_DesignerUserId1",
                table: "ProjectFilePaths",
                column: "DesignerUserId1",
                principalTable: "DesignerUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
