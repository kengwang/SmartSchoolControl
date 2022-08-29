using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolComputerControl.Server.Migrations
{
    public partial class AddClientConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PluginId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConfigId = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientConfig_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientConfig_ClientId",
                table: "ClientConfig",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientConfig");
        }
    }
}
