using Microsoft.EntityFrameworkCore.Migrations;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Migrations
{
    public partial class AddEventTypefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "AuditLogItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "AuditLogItems");
        }
    }
}
