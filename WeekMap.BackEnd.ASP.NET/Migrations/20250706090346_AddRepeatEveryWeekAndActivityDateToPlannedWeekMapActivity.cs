using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class AddRepeatEveryWeekAndActivityDateToPlannedWeekMapActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActivityDate",
                table: "PlannedWeekMapActivities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "RepeatEveryWeek",
                table: "PlannedWeekMapActivities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityDate",
                table: "PlannedWeekMapActivities");

            migrationBuilder.DropColumn(
                name: "RepeatEveryWeek",
                table: "PlannedWeekMapActivities");
        }
    }
}
