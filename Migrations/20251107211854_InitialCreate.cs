using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace musicApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscoverViewModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UnreadNotifications = table.Column<int>(type: "integer", nullable: false),
                    IsAuthenticated = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    UserEmail = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoverViewModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "downloads",
                columns: table => new
                {
                    download_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    song_id = table.Column<int>(type: "integer", nullable: false),
                    downloaded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_downloads", x => x.download_id);
                });

            migrationBuilder.CreateTable(
                name: "lyrics",
                columns: table => new
                {
                    lyrics_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    song_id = table.Column<int>(type: "integer", nullable: false),
                    lyrics_text = table.Column<string>(type: "text", nullable: true),
                    lyrics_source = table.Column<string>(type: "text", nullable: true),
                    added_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lyrics", x => x.lyrics_id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.notification_id);
                });

            migrationBuilder.CreateTable(
                name: "playlist_songs",
                columns: table => new
                {
                    playlist_song_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    playlist_id = table.Column<int>(type: "integer", nullable: false),
                    song_id = table.Column<int>(type: "integer", nullable: false),
                    order_position = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlist_songs", x => x.playlist_song_id);
                });

            migrationBuilder.CreateTable(
                name: "playlists",
                columns: table => new
                {
                    playlist_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_playlists", x => x.playlist_id);
                });

            migrationBuilder.CreateTable(
                name: "user_playback",
                columns: table => new
                {
                    playback_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    song_id = table.Column<int>(type: "integer", nullable: false),
                    current_position = table.Column<int>(type: "integer", nullable: false),
                    last_played = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_playback", x => x.playback_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    username = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    genre = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    album_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    artist = table.Column<string>(type: "text", nullable: false),
                    release_year = table.Column<int>(type: "integer", nullable: false),
                    cover_image = table.Column<string>(type: "text", nullable: true),
                    song_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DiscoverViewModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.album_id);
                    table.ForeignKey(
                        name: "FK_Album_DiscoverViewModels_DiscoverViewModelId",
                        column: x => x.DiscoverViewModelId,
                        principalTable: "DiscoverViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Song",
                columns: table => new
                {
                    song_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ArtistId = table.Column<string>(type: "text", nullable: false),
                    AlbumId = table.Column<string>(type: "text", nullable: true),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: false),
                    PlayCounts = table.Column<int>(type: "integer", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: true),
                    CoverPath = table.Column<string>(type: "text", nullable: true),
                    UploadeAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUserUploaded = table.Column<bool>(type: "boolean", nullable: false),
                    DiscoverViewModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Song", x => x.song_id);
                    table.ForeignKey(
                        name: "FK_Song_DiscoverViewModels_DiscoverViewModelId",
                        column: x => x.DiscoverViewModelId,
                        principalTable: "DiscoverViewModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "artists",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    artist_id = table.Column<int>(type: "integer", nullable: false),
                    links = table.Column<List<string>>(type: "text[]", nullable: false),
                    bio = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_artists", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_artists_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_DiscoverViewModelId",
                table: "Album",
                column: "DiscoverViewModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Song_DiscoverViewModelId",
                table: "Song",
                column: "DiscoverViewModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "artists");

            migrationBuilder.DropTable(
                name: "downloads");

            migrationBuilder.DropTable(
                name: "lyrics");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "playlist_songs");

            migrationBuilder.DropTable(
                name: "playlists");

            migrationBuilder.DropTable(
                name: "Song");

            migrationBuilder.DropTable(
                name: "user_playback");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "DiscoverViewModels");
        }
    }
}
