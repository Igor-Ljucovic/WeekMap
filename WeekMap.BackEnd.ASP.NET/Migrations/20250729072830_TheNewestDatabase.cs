using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class TheNewestDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    EmailConfirmationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmationTokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "ActivityCategories",
                columns: table => new
                {
                    ActivityCategoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityCategories", x => x.ActivityCategoryID);
                    table.ForeignKey(
                        name: "FK_ActivityCategories_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlannedWeekMaps",
                columns: table => new
                {
                    PlannedWeekMapID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ShowSaturday = table.Column<bool>(type: "bit", nullable: false),
                    ShowSunday = table.Column<bool>(type: "bit", nullable: false),
                    WeekStartDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DayEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ShowPlaceInPreview = table.Column<bool>(type: "bit", nullable: false),
                    ShowDescriptionInPreview = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlannedWeekMaps", x => x.PlannedWeekMapID);
                    table.ForeignKey(
                        name: "FK_PlannedWeekMaps_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDefaultWeekMapSettings",
                columns: table => new
                {
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    SkipSaturday = table.Column<bool>(type: "bit", nullable: false),
                    SkipSunday = table.Column<bool>(type: "bit", nullable: false),
                    WeekStartDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DayEndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ShowPlaceInPreview = table.Column<bool>(type: "bit", nullable: false),
                    ShowDescriptionInPreview = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDefaultWeekMapSettings", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserDefaultWeekMapSettings_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notification = table.Column<bool>(type: "bit", nullable: false),
                    NotificationTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EmailUpdates = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityTemplates",
                columns: table => new
                {
                    ActivityID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    ActivityCategoryID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.ActivityID);
                    table.ForeignKey(
                        name: "FK_Activities_ActivityCategories_ActivityCategoryID",
                        column: x => x.ActivityCategoryID,
                        principalTable: "ActivityCategories",
                        principalColumn: "ActivityCategoryID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Activities_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "WeekMapActivities",
                columns: table => new
                {
                    PlannedWeekMapActivityID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlannedWeekMapID = table.Column<long>(type: "bigint", nullable: false),
                    ActivityID = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RepeatEveryWeek = table.Column<bool>(type: "bit", nullable: false),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_PlannedWeekMapActivities", x => x.PlannedWeekMapActivityID);
                    table.ForeignKey(
                        name: "FK_PlannedWeekMapActivities_Activities_ActivityID",
                        column: x => x.ActivityID,
                        principalTable: "ActivityTemplates",
                        principalColumn: "ActivityTemplateID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlannedWeekMapActivities_PlannedWeekMaps_PlannedWeekMapID",
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
                    RealisedStartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    RealisedEndTime = table.Column<TimeSpan>(type: "time", nullable: false)
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
                name: "IX_Activities_ActivityCategoryID",
                table: "ActivityTemplates",
                column: "ActivityCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_UserID",
                table: "ActivityTemplates",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityCategories_UserID",
                table: "ActivityCategories",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedWeekMapActivities_ActivityID",
                table: "WeekMapActivities",
                column: "ActivityTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedWeekMapActivities_PlannedWeekMapID",
                table: "WeekMapActivities",
                column: "WeekMapID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedWeekMaps_UserID",
                table: "PlannedWeekMaps",
                column: "UserID");

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekMapActivities");

            migrationBuilder.DropTable(
                name: "RealisedWeekMapActivities");

            migrationBuilder.DropTable(
                name: "UserDefaultWeekMapSettings");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "ActivityTemplates");

            migrationBuilder.DropTable(
                name: "RealisedWeekMaps");

            migrationBuilder.DropTable(
                name: "ActivityCategories");

            migrationBuilder.DropTable(
                name: "PlannedWeekMaps");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
