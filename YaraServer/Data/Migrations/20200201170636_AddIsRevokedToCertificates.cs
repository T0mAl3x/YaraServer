using Microsoft.EntityFrameworkCore.Migrations;

namespace YaraServer.Data.Migrations
{
    public partial class AddIsRevokedToCertificates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "Certificates",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "Certificates");
        }
    }
}
