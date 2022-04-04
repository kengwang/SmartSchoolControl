using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartSchoolControl.Server.Db.Migrations
{
    public partial class InitializeDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FriendlyName = table.Column<string>(type: "TEXT", nullable: false),
                    LastHeartBeatTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedAssociations = table.Column<string>(type: "TEXT", nullable: false),
                    Permissions = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkflowExecutionFailedAction = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PluginId = table.Column<string>(type: "TEXT", nullable: false),
                    Parameters = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskActions_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Enabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    WorkflowId = table.Column<Guid>(type: "TEXT", nullable: true),
                    ClientsExecutionTimes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientScheduledTask",
                columns: table => new
                {
                    ClientsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TasksId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScheduledTask", x => new { x.ClientsId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_ClientScheduledTask_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientScheduledTask_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTriggers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<short>(type: "INTEGER", nullable: false),
                    TimesInDay = table.Column<string>(type: "TEXT", nullable: true),
                    DayInWeek = table.Column<string>(type: "TEXT", nullable: true),
                    Dates = table.Column<string>(type: "TEXT", nullable: true),
                    DateTimes = table.Column<string>(type: "TEXT", nullable: true),
                    ExecutionOnceTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ScheduledTaskId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTriggers_Tasks_ScheduledTaskId",
                        column: x => x.ScheduledTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientScheduledTask_TasksId",
                table: "ClientScheduledTask",
                column: "TasksId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskActions_WorkflowId",
                table: "TaskActions",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WorkflowId",
                table: "Tasks",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTriggers_ScheduledTaskId",
                table: "TaskTriggers",
                column: "ScheduledTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientScheduledTask");

            migrationBuilder.DropTable(
                name: "TaskActions");

            migrationBuilder.DropTable(
                name: "TaskTriggers");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Workflows");
        }
    }
}
