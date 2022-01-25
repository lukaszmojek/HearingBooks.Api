using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_TextSynthesisData_to_TextSynthesis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobContainerName",
                table: "TextSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BlobName",
                table: "TextSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "TextSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobContainerName",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "BlobName",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "TextSyntheses");
        }
    }
}
