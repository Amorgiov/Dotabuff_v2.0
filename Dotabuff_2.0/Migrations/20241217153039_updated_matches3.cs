using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dotabuff_2._0.Migrations
{
    /// <inheritdoc />
    public partial class updated_matches3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Matches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Matches",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
