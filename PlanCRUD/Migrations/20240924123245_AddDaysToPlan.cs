using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanCRUD.Migrations
{
    public partial class AddDaysToPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Days",
                table: "Plan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "Plan");
        }
    }
}
