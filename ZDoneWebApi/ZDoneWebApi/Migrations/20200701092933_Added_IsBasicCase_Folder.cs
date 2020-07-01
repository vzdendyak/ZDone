using Microsoft.EntityFrameworkCore.Migrations;

namespace ZDoneWebApi.Migrations
{
    public partial class Added_IsBasicCase_Folder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBasic",
                table: "Lists",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBasic",
                table: "Folders",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBasic",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "IsBasic",
                table: "Folders");
        }
    }
}
