using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDoneWebApi.Migrations
{
    public partial class ReasonUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Items");
        }
    }
}
