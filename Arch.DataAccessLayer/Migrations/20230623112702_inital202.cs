using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arch.DataAccessLayer.Migrations
{
    public partial class inital202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InspirationAddress1",
                table: "Competitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InspirationAddress2",
                table: "Competitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InspirationAddress1",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "InspirationAddress2",
                table: "Competitions");
        }
    }
}
