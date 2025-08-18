using Microsoft.EntityFrameworkCore.Migrations;
using WeekMap.Models;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "ActivityTemplates"
            );

            migrationBuilder.RenameTable(
                name: "PlannedWeekMaps",
                newName: "WeekMaps"
            );

            migrationBuilder.RenameTable(
                name: "PlannedWeekMapActivities",
                newName: "WeekMapActivities"
            );

            migrationBuilder.RenameColumn(
                name: "PlannedWeekMapID",
                table: "WeekMaps",
                newName: "WeekMapID"
            );

            migrationBuilder.RenameColumn(
                name: "ActivityID",
                table: "ActivityTemplates",
                newName: "ActivityTemplateID"
            );

            migrationBuilder.RenameColumn(
                name: "PlannedWeekMapID",
                table: "WeekMapActivities",
                newName: "WeekMapID"
            );

            migrationBuilder.RenameColumn(
                name: "ActivityID",
                table: "WeekMapActivities",
                newName: "ActivityTemplateID"
            );

            migrationBuilder.RenameColumn(
                name: "PlannedWeekMapActivityID",
                table: "WeekMapActivities",
                newName: "WeekMapActivityID"
            );

            migrationBuilder.RenameColumn(
                name: "PlannedWeekMapActivityID",
                table: "WeekMapActivities",
                newName: "WeekMapActivityID"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivityTemplateID",
                table: "WeekMapActivities",
                newName: "ActivityID"
            );

            migrationBuilder.RenameColumn(
                name: "WeekMapID",
                table: "WeekMapActivities",
                newName: "PlannedWeekMapID"
            );

            migrationBuilder.RenameColumn(
                name: "ActivityTemplateID",
                table: "ActivityTemplates",
                newName: "ActivityID"
            );

            migrationBuilder.RenameColumn(
                name: "WeekMapID",
                table: "WeekMaps",
                newName: "PlannedWeekMapID"
            );

            migrationBuilder.RenameColumn(
               name: "WeekMapActivityID",
               table: "WeekMapActivities",
               newName: "PlannedWeekMapActivityID"
            );

            migrationBuilder.RenameTable(
                name: "WeekMapActivities",
                newName: "PlannedWeekMapActivities"
            );

            migrationBuilder.RenameTable(
                name: "WeekMaps",
                newName: "PlannedWeekMaps"
            );

            migrationBuilder.RenameTable(
                name: "ActivityTemplates",
                newName: "Activities"
            );
        }
    }
}
