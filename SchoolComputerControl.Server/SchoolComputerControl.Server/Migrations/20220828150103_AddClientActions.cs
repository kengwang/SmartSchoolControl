using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolComputerControl.Server.Migrations
{
    public partial class AddClientActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientPluginAction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ActionName = table.Column<string>(type: "TEXT", nullable: false),
                    ActionParameter = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPluginAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ActionId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientActions_ClientPluginAction_ActionId",
                        column: x => x.ActionId,
                        principalTable: "ClientPluginAction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastHeartBeat = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    ClientActionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_ClientActions_ClientActionId",
                        column: x => x.ClientActionId,
                        principalTable: "ClientActions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientActions_ActionId",
                table: "ClientActions",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientActionId",
                table: "Clients",
                column: "ClientActionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "ClientActions");

            migrationBuilder.DropTable(
                name: "ClientPluginAction");
        }
    }
}
