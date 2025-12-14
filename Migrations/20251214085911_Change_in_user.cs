using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class Change_in_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "current_position",
                table: "user_playback");

            migrationBuilder.AddColumn<int>(
                name: "total_plays",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_plays",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "current_position",
                table: "user_playback",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
