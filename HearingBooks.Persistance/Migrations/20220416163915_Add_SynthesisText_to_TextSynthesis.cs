using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_SynthesisText_to_TextSynthesis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SynthesisText",
                table: "TextSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SynthesisText",
                table: "TextSyntheses");
        }
    }
}
