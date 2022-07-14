using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_DialogueSynthesis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DialogueSyntheses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestingUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    DialogueText = table.Column<string>(type: "text", nullable: false),
                    BlobContainerName = table.Column<string>(type: "text", nullable: false),
                    BlobName = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false),
                    FirstSpeakerVoice = table.Column<string>(type: "text", nullable: false),
                    SecondSpeakerVoice = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueSyntheses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DialogueSyntheses");
        }
    }
}
