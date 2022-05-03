using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Rename_LengthInSeconds_to_DurationInSeconds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LengthInSeconds",
                table: "TextSyntheses",
                newName: "DurationInSeconds");

            migrationBuilder.RenameColumn(
                name: "LengthInSeconds",
                table: "DialogueSyntheses",
                newName: "DurationInSeconds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DurationInSeconds",
                table: "TextSyntheses",
                newName: "LengthInSeconds");

            migrationBuilder.RenameColumn(
                name: "DurationInSeconds",
                table: "DialogueSyntheses",
                newName: "LengthInSeconds");
        }
    }
}
