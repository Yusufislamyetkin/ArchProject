using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arch.DataAccessLayer.Migrations
{
    public partial class initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectFilePathID",
                table: "Competitions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectFilePath",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitionId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectFilePath", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectFilePath_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFilePath_CompetitionId",
                table: "ProjectFilePath",
                column: "CompetitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectFilePath");

            migrationBuilder.DropColumn(
                name: "ProjectFilePathID",
                table: "Competitions");
        }
    }
}
