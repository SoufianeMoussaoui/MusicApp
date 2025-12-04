using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class Fixname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverPath",
                table: "Song",
                newName: "coverpath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "coverpath",
                table: "Song",
                newName: "CoverPath");
        }
    }
}
