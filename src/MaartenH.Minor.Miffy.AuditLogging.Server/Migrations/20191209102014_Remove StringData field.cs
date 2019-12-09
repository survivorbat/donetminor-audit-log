using Microsoft.EntityFrameworkCore.Migrations;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Migrations
{
    public partial class RemoveStringDatafield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StringData",
                table: "AuditLogItems");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "AuditLogItems",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "longblob",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "AuditLogItems",
                type: "longblob",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringData",
                table: "AuditLogItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
