using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class Change_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_plays",
                table: "users");

            migrationBuilder.AddColumn<long>(
                name: "total_seconds_played",
                table: "users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_seconds_played",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "total_plays",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
