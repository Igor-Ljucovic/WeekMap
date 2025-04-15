using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class MakeUserFieldsRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "TaskCategories",
                columns: table => new
                {
                    TaskCategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskCategories", x => x.TaskCategoryID);
                    table.ForeignKey(
                        name: "FK_TaskCategories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    UserTaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.UserTaskID);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarTasks",
                columns: table => new
                {
                    UserTaskID = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    RemindMeNotification = table.Column<bool>(type: "bit", nullable: false),
                    RemindMeEmail = table.Column<bool>(type: "bit", nullable: false),
                    HoursBeforeNotification = table.Column<int>(type: "int", nullable: true),
                    HoursBeforeEmail = table.Column<int>(type: "int", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarTasks", x => x.UserTaskID);
                    table.ForeignKey(
                        name: "FK_CalendarTasks_Tasks_UserTaskID",
                        column: x => x.UserTaskID,
                        principalTable: "Tasks",
                        principalColumn: "UserTaskID");
                });

            migrationBuilder.CreateTable(
                name: "NoDeadlineTasks",
                columns: table => new
                {
                    UserTaskID = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoDeadlineTasks", x => x.UserTaskID);
                    table.ForeignKey(
                        name: "FK_NoDeadlineTasks_Tasks_UserTaskID",
                        column: x => x.UserTaskID,
                        principalTable: "Tasks",
                        principalColumn: "UserTaskID");
                });

            migrationBuilder.CreateTable(
                name: "RepetitiveTasks",
                columns: table => new
                {
                    UserTaskID = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weekly = table.Column<bool>(type: "bit", nullable: false),
                    Daily = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OnMonday = table.Column<bool>(type: "bit", nullable: false),
                    OnTuesday = table.Column<bool>(type: "bit", nullable: false),
                    OnWednesday = table.Column<bool>(type: "bit", nullable: false),
                    OnThursday = table.Column<bool>(type: "bit", nullable: false),
                    OnFriday = table.Column<bool>(type: "bit", nullable: false),
                    OnSaturday = table.Column<bool>(type: "bit", nullable: false),
                    OnSunday = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepetitiveTasks", x => x.UserTaskID);
                    table.ForeignKey(
                        name: "FK_RepetitiveTasks_Tasks_UserTaskID",
                        column: x => x.UserTaskID,
                        principalTable: "Tasks",
                        principalColumn: "UserTaskID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskCategories_UserID",
                table: "TaskCategories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserID",
                table: "Tasks",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalendarTasks");

            migrationBuilder.DropTable(
                name: "NoDeadlineTasks");

            migrationBuilder.DropTable(
                name: "RepetitiveTasks");

            migrationBuilder.DropTable(
                name: "TaskCategories");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
