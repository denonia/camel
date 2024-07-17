using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Camel.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddBeatmapsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beatmaps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mapset_id = table.Column<int>(type: "integer", nullable: false),
                    md5 = table.Column<string>(type: "text", nullable: false),
                    artist = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<string>(type: "text", nullable: false),
                    creator = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    last_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total_length = table.Column<int>(type: "integer", nullable: false),
                    max_combo = table.Column<int>(type: "integer", nullable: false),
                    frozen = table.Column<bool>(type: "boolean", nullable: false),
                    plays = table.Column<int>(type: "integer", nullable: false),
                    passes = table.Column<int>(type: "integer", nullable: false),
                    mode = table.Column<byte>(type: "smallint", nullable: false),
                    bpm = table.Column<float>(type: "real", nullable: false),
                    circle_size = table.Column<float>(type: "real", nullable: false),
                    approach_rate = table.Column<float>(type: "real", nullable: false),
                    overall_difficulty = table.Column<float>(type: "real", nullable: false),
                    hp_drain = table.Column<float>(type: "real", nullable: false),
                    star_rate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_beatmaps", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_md5",
                table: "beatmaps",
                column: "md5",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beatmaps");
        }
    }
}
