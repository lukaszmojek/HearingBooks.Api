using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HearingBooks.Persistance.Migrations
{
    public partial class Add_SynthesisPricing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PriceInUsd",
                table: "TextSyntheses",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceInUsd",
                table: "DialogueSyntheses",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "SynthesisPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SynthesisType = table.Column<int>(type: "integer", nullable: false),
                    PriceInUsdPer1MCharacters = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynthesisPricings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SynthesisPricings");

            migrationBuilder.DropColumn(
                name: "PriceInUsd",
                table: "TextSyntheses");

            migrationBuilder.DropColumn(
                name: "PriceInUsd",
                table: "DialogueSyntheses");
        }
    }
}
