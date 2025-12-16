using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class Change_song : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Song_artist_id",
                table: "Song",
                column: "artist_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_artists_artist_id",
                table: "Song",
                column: "artist_id",
                principalTable: "artists",
                principalColumn: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Song_artists_artist_id",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Song_artist_id",
                table: "Song");
        }
    }
}
