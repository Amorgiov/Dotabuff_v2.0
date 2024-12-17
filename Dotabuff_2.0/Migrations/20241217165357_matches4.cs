using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotabuff_2._0.Migrations
{
    /// <inheritdoc />
    public partial class matches4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "DireHeroes",
                table: "Matches",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "RadiantHeroes",
                table: "Matches",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DireHeroes",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "RadiantHeroes",
                table: "Matches");
        }
    }
}
