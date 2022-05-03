using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_CharacterCouint_and_LengthInSeconds_to_TextSynthesis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CharacterCount",
                table: "TextSyntheses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LengthInSeconds",
                table: "TextSyntheses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CharacterCount",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "LengthInSeconds",
                table: "TextSyntheses");
        }
    }
}
