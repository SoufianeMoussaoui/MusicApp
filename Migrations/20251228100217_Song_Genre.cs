using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class Song_Genre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "genre",
                table: "Song",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchTerm",
                table: "DiscoverViewModels",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "genre",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "SearchTerm",
                table: "DiscoverViewModels");
        }
    }
}
