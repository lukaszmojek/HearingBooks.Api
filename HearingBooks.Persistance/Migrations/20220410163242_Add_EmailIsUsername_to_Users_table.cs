using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class AddEmailIsUsernametoUserstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailIsUsername",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailIsUsername",
                table: "Users");
        }
    }
}
