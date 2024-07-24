using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Camel.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesAndLoginSessionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "country",
                table: "users",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "XX");

            migrationBuilder.CreateTable(
                name: "login_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    osu_version = table.Column<string>(type: "text", nullable: false),
                    running_under_wine = table.Column<bool>(type: "boolean", nullable: false),
                    osu_path_md5 = table.Column<string>(type: "text", nullable: true),
                    adapters_str = table.Column<string>(type: "text", nullable: true),
                    adapters_md5 = table.Column<string>(type: "text", nullable: true),
                    uninstall_md5 = table.Column<string>(type: "text", nullable: true),
                    disk_signature_md5 = table.Column<string>(type: "text", nullable: true),
                    ip_address = table.Column<string>(type: "text", nullable: false),
                    date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_login_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_login_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_stats_mode",
                table: "stats",
                column: "mode");

            migrationBuilder.CreateIndex(
                name: "ix_stats_pp",
                table: "stats",
                column: "pp");

            migrationBuilder.CreateIndex(
                name: "ix_stats_ranked_score",
                table: "stats",
                column: "ranked_score");

            migrationBuilder.CreateIndex(
                name: "ix_stats_total_score",
                table: "stats",
                column: "total_score");

            migrationBuilder.CreateIndex(
                name: "ix_scores_map_md5",
                table: "scores",
                column: "map_md5");

            migrationBuilder.CreateIndex(
                name: "ix_scores_mode",
                table: "scores",
                column: "mode");

            migrationBuilder.CreateIndex(
                name: "ix_scores_mods",
                table: "scores",
                column: "mods");

            migrationBuilder.CreateIndex(
                name: "ix_scores_online_checksum",
                table: "scores",
                column: "online_checksum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_scores_pp",
                table: "scores",
                column: "pp");

            migrationBuilder.CreateIndex(
                name: "ix_scores_score_num",
                table: "scores",
                column: "score_num");

            migrationBuilder.CreateIndex(
                name: "ix_scores_status",
                table: "scores",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_file_name",
                table: "beatmaps",
                column: "file_name");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_mode",
                table: "beatmaps",
                column: "mode");

            migrationBuilder.CreateIndex(
                name: "ix_login_sessions_date_time",
                table: "login_sessions",
                column: "date_time");

            migrationBuilder.CreateIndex(
                name: "ix_login_sessions_osu_version",
                table: "login_sessions",
                column: "osu_version");

            migrationBuilder.CreateIndex(
                name: "ix_login_sessions_user_id",
                table: "login_sessions",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_sessions");

            migrationBuilder.DropIndex(
                name: "ix_stats_mode",
                table: "stats");

            migrationBuilder.DropIndex(
                name: "ix_stats_pp",
                table: "stats");

            migrationBuilder.DropIndex(
                name: "ix_stats_ranked_score",
                table: "stats");

            migrationBuilder.DropIndex(
                name: "ix_stats_total_score",
                table: "stats");

            migrationBuilder.DropIndex(
                name: "ix_scores_map_md5",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_scores_mode",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_scores_mods",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_scores_online_checksum",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_scores_pp",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_scores_score_num",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_scores_status",
                table: "scores");

            migrationBuilder.DropIndex(
                name: "ix_beatmaps_file_name",
                table: "beatmaps");

            migrationBuilder.DropIndex(
                name: "ix_beatmaps_mode",
                table: "beatmaps");

            migrationBuilder.DropColumn(
                name: "country",
                table: "users");
        }
    }
}
