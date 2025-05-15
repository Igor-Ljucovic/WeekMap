using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class AddPrimaryKeyToPlannedWeekMapActivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities");

            migrationBuilder.DropIndex(
                name: "IX_RealisedWeekMapActivities_RealisedWeekMapID",
                table: "RealisedWeekMapActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities");

            migrationBuilder.DropIndex(
                name: "IX_PlannedWeekMapActivities_PlannedWeekMapID",
                table: "PlannedWeekMapActivities");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                table: "UserSettings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "WeekStartDay",
                table: "UserDefaultWeekMapSettings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<long>(
                name: "PlannedWeekMapActivityID",
                table: "PlannedWeekMapActivities",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities",
                columns: new[] { "RealisedWeekMapID", "ActivityID", "RealisedStartTime", "RealisedEndTime" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities",
                columns: new[] { "PlannedWeekMapID", "ActivityID", "StartTime", "EndTime", "OnMonday", "OnTuesday", "OnWednesday", "OnThursday", "OnFriday", "OnSaturday", "OnSunday" });

            migrationBuilder.CreateIndex(
                name: "IX_RealisedWeekMapActivities_ActivityID",
                table: "RealisedWeekMapActivities",
                column: "ActivityID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedWeekMapActivities_ActivityID",
                table: "PlannedWeekMapActivities",
                column: "ActivityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities");

            migrationBuilder.DropIndex(
                name: "IX_RealisedWeekMapActivities_ActivityID",
                table: "RealisedWeekMapActivities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities");

            migrationBuilder.DropIndex(
                name: "IX_PlannedWeekMapActivities_ActivityID",
                table: "PlannedWeekMapActivities");

            migrationBuilder.DropColumn(
                name: "PlannedWeekMapActivityID",
                table: "PlannedWeekMapActivities");

            migrationBuilder.AlterColumn<string>(
                name: "Theme",
                table: "UserSettings",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "WeekStartDay",
                table: "UserDefaultWeekMapSettings",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RealisedWeekMapActivities",
                table: "RealisedWeekMapActivities",
                columns: new[] { "ActivityID", "RealisedWeekMapID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlannedWeekMapActivities",
                table: "PlannedWeekMapActivities",
                columns: new[] { "ActivityID", "PlannedWeekMapID" });

            migrationBuilder.CreateIndex(
                name: "IX_RealisedWeekMapActivities_RealisedWeekMapID",
                table: "RealisedWeekMapActivities",
                column: "RealisedWeekMapID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedWeekMapActivities_PlannedWeekMapID",
                table: "PlannedWeekMapActivities",
                column: "PlannedWeekMapID");
        }
    }
}
