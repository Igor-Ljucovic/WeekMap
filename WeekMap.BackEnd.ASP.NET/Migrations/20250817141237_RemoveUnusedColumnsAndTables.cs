using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class RemoveUnusedColumnsAndTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RealisedWeekMapActivities");

            migrationBuilder.DropTable(
                name: "RealisedWeekMaps");

            migrationBuilder.DropColumn(
                name: "EmailUpdates",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "Notification",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "NotificationTime",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "SkipSaturday",
                table: "UserDefaultWeekMapSettings");

            migrationBuilder.DropColumn(
                name: "SkipSunday",
                table: "UserDefaultWeekMapSettings");

            migrationBuilder.DropColumn(
                name: "WeekStartDay",
                table: "UserDefaultWeekMapSettings");

            migrationBuilder.DropColumn(
                name: "ShowSaturday",
                table: "PlannedWeekMaps");

            migrationBuilder.DropColumn(
                name: "ShowSunday",
                table: "PlannedWeekMaps");

            migrationBuilder.DropColumn(
                name: "WeekStartDay",
                table: "PlannedWeekMaps");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailUpdates",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Notification",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "NotificationTime",
                table: "UserSettings",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "SkipSaturday",
                table: "UserDefaultWeekMapSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SkipSunday",
                table: "UserDefaultWeekMapSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WeekStartDay",
                table: "UserDefaultWeekMapSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ShowSaturday",
                table: "PlannedWeekMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowSunday",
                table: "PlannedWeekMaps",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WeekStartDay",
                table: "PlannedWeekMaps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RealisedWeekMaps",
                columns: table => new
                {
                    RealisedWeekMapID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlannedWeekMapID = table.Column<long>(type: "bigint", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealisedWeekMaps", x => x.RealisedWeekMapID);
                    table.ForeignKey(
                        name: "FK_RealisedWeekMaps_PlannedWeekMaps_PlannedWeekMapID",
                        column: x => x.PlannedWeekMapID,
                        principalTable: "PlannedWeekMaps",
                        principalColumn: "WeekMapID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RealisedWeekMapActivities",
                columns: table => new
                {
                    RealisedWeekMapActivityID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityID = table.Column<long>(type: "bigint", nullable: false),
                    RealisedWeekMapID = table.Column<long>(type: "bigint", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    RealisedEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RealisedStartTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealisedWeekMapActivities", x => x.RealisedWeekMapActivityID);
                    table.ForeignKey(
                        name: "FK_RealisedWeekMapActivities_Activities_ActivityID",
                        column: x => x.ActivityID,
                        principalTable: "ActivityTemplates",
                        principalColumn: "ActivityTemplateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealisedWeekMapActivities_RealisedWeekMaps_RealisedWeekMapID",
                        column: x => x.RealisedWeekMapID,
                        principalTable: "RealisedWeekMaps",
                        principalColumn: "RealisedWeekMapID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealisedWeekMapActivities_ActivityID",
                table: "RealisedWeekMapActivities",
                column: "ActivityTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_RealisedWeekMapActivities_RealisedWeekMapID",
                table: "RealisedWeekMapActivities",
                column: "RealisedWeekMapID");

            migrationBuilder.CreateIndex(
                name: "IX_RealisedWeekMaps_PlannedWeekMapID",
                table: "RealisedWeekMaps",
                column: "WeekMapID");
        }
    }
}
