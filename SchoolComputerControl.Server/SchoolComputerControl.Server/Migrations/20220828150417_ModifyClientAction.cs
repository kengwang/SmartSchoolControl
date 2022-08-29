using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolComputerControl.Server.Migrations
{
    public partial class ModifyClientAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientActions_ClientPluginAction_ActionId",
                table: "ClientActions");

            migrationBuilder.DropIndex(
                name: "IX_ClientActions_ActionId",
                table: "ClientActions");

            migrationBuilder.DropColumn(
                name: "ActionId",
                table: "ClientActions");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientActionId",
                table: "ClientPluginAction",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "ClientActions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "ClientActions",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ClientPluginAction_ClientActionId",
                table: "ClientPluginAction",
                column: "ClientActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPluginAction_ClientActions_ClientActionId",
                table: "ClientPluginAction",
                column: "ClientActionId",
                principalTable: "ClientActions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPluginAction_ClientActions_ClientActionId",
                table: "ClientPluginAction");

            migrationBuilder.DropIndex(
                name: "IX_ClientPluginAction_ClientActionId",
                table: "ClientPluginAction");

            migrationBuilder.DropColumn(
                name: "ClientActionId",
                table: "ClientPluginAction");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "ClientActions");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "ClientActions");

            migrationBuilder.AddColumn<string>(
                name: "ActionId",
                table: "ClientActions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientActions_ActionId",
                table: "ClientActions",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientActions_ClientPluginAction_ActionId",
                table: "ClientActions",
                column: "ActionId",
                principalTable: "ClientPluginAction",
                principalColumn: "Id");
        }
    }
}
