using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class ChangedPlannedWeekMapFieldsToRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowSaturdayAndSunday",
                table: "PlannedWeekMaps",
                newName: "ShowSunday");

            migrationBuilder.AlterColumn<string>(
                name: "WeekStartDay",
                table: "PlannedWeekMaps",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "ActivityCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(6)",
                oldMaxLength: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowSunday",
                table: "PlannedWeekMaps",
                newName: "ShowSaturdayAndSunday");

            migrationBuilder.AlterColumn<string>(
                name: "WeekStartDay",
                table: "PlannedWeekMaps",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "ActivityCategories",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
