using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class ChangeColumnsName2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowLocationInPreview",
                table: "WeekMaps",
                newName: "ShowPlaceInPreview");

            migrationBuilder.RenameColumn(
               name: "ShowLocationInPreview",
               table: "UserDefaultWeekMapSettings",
               newName: "ShowPlaceInPreview");

            migrationBuilder.RenameIndex(
               name: "IX_PlannedWeekMaps_UserID",
               table: "WeekMaps",
               newName: "IX_WeekMaps_UserID");
        }
    }
}
