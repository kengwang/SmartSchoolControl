using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolComputerControl.Server.Migrations
{
    public partial class UseClientConfigConver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientConfig");

            migrationBuilder.AddColumn<string>(
                name: "Configs",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Configs",
                table: "Clients");

            migrationBuilder.CreateTable(
                name: "ClientConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClientId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ConfigId = table.Column<string>(type: "TEXT", nullable: false),
                    PluginId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
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
    }
}
