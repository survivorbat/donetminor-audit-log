using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogItems",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogItems");
        }
    }
}
