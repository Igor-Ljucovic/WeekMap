using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class UpdateUniqueEmailConstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarTasks_Tasks_UserTaskID",
                table: "CalendarTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_NoDeadlineTasks_Tasks_UserTaskID",
                table: "NoDeadlineTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_RepetitiveTasks_Tasks_UserTaskID",
                table: "RepetitiveTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserID",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "UserTasks");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserID",
                table: "UserTasks",
                newName: "IX_UserTasks_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks",
                column: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarTasks_UserTasks_UserTaskID",
                table: "CalendarTasks",
                column: "UserTaskID",
                principalTable: "UserTasks",
                principalColumn: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_NoDeadlineTasks_UserTasks_UserTaskID",
                table: "NoDeadlineTasks",
                column: "UserTaskID",
                principalTable: "UserTasks",
                principalColumn: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_RepetitiveTasks_UserTasks_UserTaskID",
                table: "RepetitiveTasks",
                column: "UserTaskID",
                principalTable: "UserTasks",
                principalColumn: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_Users_UserID",
                table: "UserTasks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarTasks_UserTasks_UserTaskID",
                table: "CalendarTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_NoDeadlineTasks_UserTasks_UserTaskID",
                table: "NoDeadlineTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_RepetitiveTasks_UserTasks_UserTaskID",
                table: "RepetitiveTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_Users_UserID",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks");

            migrationBuilder.RenameTable(
                name: "UserTasks",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_UserID",
                table: "Tasks",
                newName: "IX_Tasks_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarTasks_Tasks_UserTaskID",
                table: "CalendarTasks",
                column: "UserTaskID",
                principalTable: "Tasks",
                principalColumn: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_NoDeadlineTasks_Tasks_UserTaskID",
                table: "NoDeadlineTasks",
                column: "UserTaskID",
                principalTable: "Tasks",
                principalColumn: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_RepetitiveTasks_Tasks_UserTaskID",
                table: "RepetitiveTasks",
                column: "UserTaskID",
                principalTable: "Tasks",
                principalColumn: "UserTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserID",
                table: "Tasks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
