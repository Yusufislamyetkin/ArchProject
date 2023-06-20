using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arch.DataAccessLayer.Migrations
{
    public partial class initial103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RewardId",
                table: "Competitions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelectedOption = table.Column<int>(type: "int", nullable: true),
                    DesignerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompetitionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rewards_AspNetUsers_DesignerId",
                        column: x => x.DesignerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rewards_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_CompetitionId",
                table: "Rewards",
                column: "CompetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_DesignerId",
                table: "Rewards",
                column: "DesignerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropColumn(
                name: "RewardId",
                table: "Competitions");
        }
    }
}
