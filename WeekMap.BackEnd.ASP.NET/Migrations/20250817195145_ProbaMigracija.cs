using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeekMap.Migrations
{
    public partial class ProbaMigracija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlannedWeekMapActivityID",
                table: "WeekMapActivities",
                newName: "WeekMapActivityID"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
               name: "WeekMapActivityID",
               table: "WeekMapActivities",
               newName: "PlannedWeekMapActivityID"
            );
        }
    }
}
