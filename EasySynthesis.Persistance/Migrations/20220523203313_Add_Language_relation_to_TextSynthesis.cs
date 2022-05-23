using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_Language_relation_to_TextSynthesis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailIsUsername",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailNotificationsEnabled",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "TextSyntheses");

            migrationBuilder.AddColumn<Guid>(
                name: "PreferenceId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "TextSyntheses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotificationsEnabled",
                table: "Preferences",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PreferenceId",
                table: "Users",
                column: "PreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_TextSyntheses_LanguageId",
                table: "TextSyntheses",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TextSyntheses_Languages_LanguageId",
                table: "TextSyntheses",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Preferences_PreferenceId",
                table: "Users",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextSyntheses_Languages_LanguageId",
                table: "TextSyntheses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Preferences_PreferenceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PreferenceId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TextSyntheses_LanguageId",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "PreferenceId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "EmailNotificationsEnabled",
                table: "Preferences");

            migrationBuilder.AddColumn<bool>(
                name: "EmailIsUsername",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotificationsEnabled",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "TextSyntheses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
