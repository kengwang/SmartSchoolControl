using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolComputerControl.Server.Migrations
{
    public partial class AddAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPluginAction");

            migrationBuilder.AddColumn<string>(
                name: "Actions",
                table: "Actions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actions",
                table: "Actions");

            migrationBuilder.CreateTable(
                name: "ClientPluginAction",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ActionName = table.Column<string>(type: "TEXT", nullable: false),
                    ActionParameter = table.Column<string>(type: "TEXT", nullable: true),
                    ClientActionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPluginAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPluginAction_Actions_ClientActionId",
                        column: x => x.ClientActionId,
                        principalTable: "Actions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientPluginAction_ClientActionId",
                table: "ClientPluginAction",
                column: "ClientActionId");
        }
    }
}
