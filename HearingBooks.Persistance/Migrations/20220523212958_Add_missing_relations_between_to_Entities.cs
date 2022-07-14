using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_missing_relations_between_to_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Voice",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "FirstSpeakerVoice",
                table: "DialogueSyntheses");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "DialogueSyntheses");

            migrationBuilder.DropColumn(
                name: "SecondSpeakerVoice",
                table: "DialogueSyntheses");

            migrationBuilder.RenameColumn(
                name: "RequestingUserId",
                table: "TextSyntheses",
                newName: "VoiceId");

            migrationBuilder.RenameColumn(
                name: "RequestingUserId",
                table: "DialogueSyntheses",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TextSyntheses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FirstSpeakerVoiceId",
                table: "DialogueSyntheses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "DialogueSyntheses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SecondSpeakerVoiceId",
                table: "DialogueSyntheses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TextSyntheses_UserId",
                table: "TextSyntheses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TextSyntheses_VoiceId",
                table: "TextSyntheses",
                column: "VoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueSyntheses_FirstSpeakerVoiceId",
                table: "DialogueSyntheses",
                column: "FirstSpeakerVoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueSyntheses_LanguageId",
                table: "DialogueSyntheses",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueSyntheses_SecondSpeakerVoiceId",
                table: "DialogueSyntheses",
                column: "SecondSpeakerVoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueSyntheses_UserId",
                table: "DialogueSyntheses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DialogueSyntheses_Languages_LanguageId",
                table: "DialogueSyntheses",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DialogueSyntheses_Users_UserId",
                table: "DialogueSyntheses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DialogueSyntheses_Voices_FirstSpeakerVoiceId",
                table: "DialogueSyntheses",
                column: "FirstSpeakerVoiceId",
                principalTable: "Voices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DialogueSyntheses_Voices_SecondSpeakerVoiceId",
                table: "DialogueSyntheses",
                column: "SecondSpeakerVoiceId",
                principalTable: "Voices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextSyntheses_Users_UserId",
                table: "TextSyntheses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TextSyntheses_Voices_VoiceId",
                table: "TextSyntheses",
                column: "VoiceId",
                principalTable: "Voices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DialogueSyntheses_Languages_LanguageId",
                table: "DialogueSyntheses");

            migrationBuilder.DropForeignKey(
                name: "FK_DialogueSyntheses_Users_UserId",
                table: "DialogueSyntheses");

            migrationBuilder.DropForeignKey(
                name: "FK_DialogueSyntheses_Voices_FirstSpeakerVoiceId",
                table: "DialogueSyntheses");

            migrationBuilder.DropForeignKey(
                name: "FK_DialogueSyntheses_Voices_SecondSpeakerVoiceId",
                table: "DialogueSyntheses");

            migrationBuilder.DropForeignKey(
                name: "FK_TextSyntheses_Users_UserId",
                table: "TextSyntheses");

            migrationBuilder.DropForeignKey(
                name: "FK_TextSyntheses_Voices_VoiceId",
                table: "TextSyntheses");

            migrationBuilder.DropIndex(
                name: "IX_TextSyntheses_UserId",
                table: "TextSyntheses");

            migrationBuilder.DropIndex(
                name: "IX_TextSyntheses_VoiceId",
                table: "TextSyntheses");

            migrationBuilder.DropIndex(
                name: "IX_DialogueSyntheses_FirstSpeakerVoiceId",
                table: "DialogueSyntheses");

            migrationBuilder.DropIndex(
                name: "IX_DialogueSyntheses_LanguageId",
                table: "DialogueSyntheses");

            migrationBuilder.DropIndex(
                name: "IX_DialogueSyntheses_SecondSpeakerVoiceId",
                table: "DialogueSyntheses");

            migrationBuilder.DropIndex(
                name: "IX_DialogueSyntheses_UserId",
                table: "DialogueSyntheses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "FirstSpeakerVoiceId",
                table: "DialogueSyntheses");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "DialogueSyntheses");

            migrationBuilder.DropColumn(
                name: "SecondSpeakerVoiceId",
                table: "DialogueSyntheses");

            migrationBuilder.RenameColumn(
                name: "VoiceId",
                table: "TextSyntheses",
                newName: "RequestingUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "DialogueSyntheses",
                newName: "RequestingUserId");

            migrationBuilder.AddColumn<string>(
                name: "Voice",
                table: "TextSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstSpeakerVoice",
                table: "DialogueSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "DialogueSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondSpeakerVoice",
                table: "DialogueSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
