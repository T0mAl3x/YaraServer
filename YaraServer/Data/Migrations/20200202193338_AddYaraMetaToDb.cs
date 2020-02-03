using Microsoft.EntityFrameworkCore.Migrations;

namespace YaraServer.Data.Migrations
{
    public partial class AddYaraMetaToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "YaraMetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MetaKey = table.Column<string>(nullable: true),
                    MetaValue = table.Column<string>(nullable: true),
                    YaraResultId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YaraMetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YaraMetas_YaraResults_YaraResultId",
                        column: x => x.YaraResultId,
                        principalTable: "YaraResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YaraMetas_YaraResultId",
                table: "YaraMetas",
                column: "YaraResultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YaraMetas");
        }
    }
}
