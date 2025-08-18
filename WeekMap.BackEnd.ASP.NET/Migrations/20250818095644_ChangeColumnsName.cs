using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class ChangeColumnsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "ShowPlaceInPreview", 
               table: "WeekMaps",
               newName: "ShowLocationInPreview");

            migrationBuilder.RenameColumn(
               name: "ShowPlaceInPreview",
               table: "DefaultWeekMapSettings",
               newName: "ShowLocationInPreview");

            migrationBuilder.RenameIndex(
                name: "IX_WeekMaps_UserID", 
                table: "WeekMaps",
                newName: "IX_PlannedWeekMaps_UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowLocationInPreview",
                table: "WeekMaps",
                newName: "ShowPlaceInPreview");
        }
    }
}
