using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class disc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_DiscoverViewModels_DiscoverViewModelId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Song_DiscoverViewModels_DiscoverViewModelId",
                table: "Song");

            migrationBuilder.DropTable(
                name: "DiscoverViewModels");

            migrationBuilder.DropIndex(
                name: "IX_Song_DiscoverViewModelId",
                table: "Song");

            migrationBuilder.DropIndex(
                name: "IX_Album_DiscoverViewModelId",
                table: "Album");

            migrationBuilder.DropColumn(
                name: "DiscoverViewModelId",
                table: "Song");

            migrationBuilder.DropColumn(
                name: "DiscoverViewModelId",
                table: "Album");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscoverViewModelId",
                table: "Song",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscoverViewModelId",
                table: "Album",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscoverViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAuthenticated = table.Column<bool>(type: "boolean", nullable: false),
                    SearchTerm = table.Column<string>(type: "text", nullable: false),
                    UnreadNotifications = table.Column<int>(type: "integer", nullable: false),
                    UserEmail = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoverViewModels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Song_DiscoverViewModelId",
                table: "Song",
                column: "DiscoverViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Album_DiscoverViewModelId",
                table: "Album",
                column: "DiscoverViewModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_DiscoverViewModels_DiscoverViewModelId",
                table: "Album",
                column: "DiscoverViewModelId",
                principalTable: "DiscoverViewModels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Song_DiscoverViewModels_DiscoverViewModelId",
                table: "Song",
                column: "DiscoverViewModelId",
                principalTable: "DiscoverViewModels",
                principalColumn: "Id");
        }
    }
}
