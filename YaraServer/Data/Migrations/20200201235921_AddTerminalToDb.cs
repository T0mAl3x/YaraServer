using Microsoft.EntityFrameworkCore.Migrations;

namespace YaraServer.Data.Migrations
{
    public partial class AddTerminalToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Terminals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemName = table.Column<string>(nullable: true),
                    OsName = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    MAC = table.Column<string>(nullable: true),
                    Processor = table.Column<string>(nullable: true),
                    Motherboard = table.Column<string>(nullable: true),
                    RAM = table.Column<string>(nullable: true),
                    CertificateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terminals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Terminals_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Terminals_CertificateId",
                table: "Terminals",
                column: "CertificateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Terminals");
        }
    }
}
