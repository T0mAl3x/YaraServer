using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace YaraServer.Data.Migrations
{
    public partial class AddReportModelToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScanId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    SHA1 = table.Column<string>(nullable: true),
                    SHA256 = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(nullable: true),
                    Positives = table.Column<int>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Tag = table.Column<int>(nullable: false),
                    TerminalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Terminals_TerminalId",
                        column: x => x.TerminalId,
                        principalTable: "Terminals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TerminalId",
                table: "Reports",
                column: "TerminalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
