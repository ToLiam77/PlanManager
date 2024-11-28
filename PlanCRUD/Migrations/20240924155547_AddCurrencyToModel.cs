using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanCRUD.Migrations
{
    public partial class AddCurrencyToModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Plan",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Plan");
        }
    }
}
