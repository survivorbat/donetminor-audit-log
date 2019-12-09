using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Migrations
{
    public partial class Newfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "AuditLogItems");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Data",
                table: "AuditLogItems",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringData",
                table: "AuditLogItems",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TimeStamp",
                table: "AuditLogItems",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "AuditLogItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StringData",
                table: "AuditLogItems");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "AuditLogItems");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "AuditLogItems");

            migrationBuilder.AlterColumn<string>(
                name: "Data",
                table: "AuditLogItems",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "AuditLogItems",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
