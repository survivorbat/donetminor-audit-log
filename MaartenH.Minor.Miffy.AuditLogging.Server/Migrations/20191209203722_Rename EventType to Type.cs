using Microsoft.EntityFrameworkCore.Migrations;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Migrations
{
    public partial class RenameEventTypetoType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "AuditLogItems");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AuditLogItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "AuditLogItems");

            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "AuditLogItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
