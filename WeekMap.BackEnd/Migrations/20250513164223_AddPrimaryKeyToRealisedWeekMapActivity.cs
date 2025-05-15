using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class AddPrimaryKeyToRealisedWeekMapActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities");

            migrationBuilder.AddColumn<long>(
                name: "RealisedWeekMapActivityID",
                table: "RealisedWeekMapActivities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities",
                columns: new[] { "RealisedWeekMapID", "ActivityID", "RealisedWeekMapActivityID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities",
                columns: new[] { "PlannedWeekMapID", "ActivityID", "PlannedWeekMapActivityID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities");

            migrationBuilder.DropColumn(
                name: "RealisedWeekMapActivityID",
                table: "RealisedWeekMapActivities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities",
                columns: new[] { "RealisedWeekMapID", "ActivityID", "RealisedStartTime", "RealisedEndTime" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities",
                columns: new[] { "PlannedWeekMapID", "ActivityID", "StartTime", "EndTime", "OnMonday", "OnTuesday", "OnWednesday", "OnThursday", "OnFriday", "OnSaturday", "OnSunday" });
        }
    }
}
