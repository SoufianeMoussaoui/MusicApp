using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class Navigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArtistId",
                table: "Song",
                newName: "artist_id");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Song",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "artist_id",
                table: "Song",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "artist_id",
                table: "Song",
                newName: "ArtistId");

            migrationBuilder.AlterColumn<string>(
                name: "AlbumId",
                table: "Song",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ArtistId",
                table: "Song",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
