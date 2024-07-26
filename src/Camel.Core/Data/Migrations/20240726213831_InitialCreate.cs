using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Camel.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    md5 = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    artist = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    artist_unicode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    title_unicode = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    version = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    creator = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    source = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    file_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    last_update = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    approved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    star_rate = table.Column<float>(type: "real", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    beatmap_source = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_beatmaps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    country = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, defaultValue: "XX"),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_role_claims_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "login_sessions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    osu_version = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    running_under_wine = table.Column<bool>(type: "boolean", nullable: false),
                    osu_path_md5 = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    adapters_str = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    adapters_md5 = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    uninstall_md5 = table.Column<string>(type: "text", nullable: true),
                    disk_signature_md5 = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    ip_address = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "profiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    twitter = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    discord = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    user_page = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_profiles_users_id",
                        column: x => x.id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "relationships",
                columns: table => new
                {
                    first_user_id = table.Column<int>(type: "integer", nullable: false),
                    second_user_id = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_relationships", x => new { x.first_user_id, x.second_user_id });
                    table.ForeignKey(
                        name: "fk_relationships_users_first_user_id",
                        column: x => x.first_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_relationships_users_second_user_id",
                        column: x => x.second_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    mode = table.Column<byte>(type: "smallint", nullable: false),
                    total_score = table.Column<long>(type: "bigint", nullable: false),
                    ranked_score = table.Column<long>(type: "bigint", nullable: false),
                    pp = table.Column<short>(type: "smallint", nullable: false),
                    plays = table.Column<int>(type: "integer", nullable: false),
                    play_time = table.Column<int>(type: "integer", nullable: false),
                    accuracy = table.Column<float>(type: "real", nullable: false),
                    max_combo = table.Column<int>(type: "integer", nullable: false),
                    total_hits = table.Column<int>(type: "integer", nullable: false),
                    replay_views = table.Column<int>(type: "integer", nullable: false),
                    xh_count = table.Column<int>(type: "integer", nullable: false),
                    x_count = table.Column<int>(type: "integer", nullable: false),
                    sh_count = table.Column<int>(type: "integer", nullable: false),
                    s_count = table.Column<int>(type: "integer", nullable: false),
                    a_count = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stats", x => x.id);
                    table.ForeignKey(
                        name: "fk_stats_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_claims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_claims_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_logins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_logins", x => new { x.provider_key, x.login_provider });
                    table.ForeignKey(
                        name: "fk_user_logins_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scores",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    score_num = table.Column<int>(type: "integer", nullable: false),
                    pp = table.Column<float>(type: "real", nullable: false),
                    accuracy = table.Column<float>(type: "real", nullable: false),
                    max_combo = table.Column<int>(type: "integer", nullable: false),
                    mods = table.Column<int>(type: "integer", nullable: false),
                    count300 = table.Column<int>(type: "integer", nullable: false),
                    count100 = table.Column<int>(type: "integer", nullable: false),
                    count50 = table.Column<int>(type: "integer", nullable: false),
                    count_miss = table.Column<int>(type: "integer", nullable: false),
                    count_geki = table.Column<int>(type: "integer", nullable: false),
                    count_katu = table.Column<int>(type: "integer", nullable: false),
                    grade = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    mode = table.Column<byte>(type: "smallint", nullable: false),
                    set_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    time_elapsed = table.Column<int>(type: "integer", nullable: false),
                    client_flags = table.Column<int>(type: "integer", nullable: false),
                    perfect = table.Column<bool>(type: "boolean", nullable: false),
                    online_checksum = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    map_md5 = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    session_id = table.Column<int>(type: "integer", nullable: false),
                    beatmap_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_scores", x => x.id);
                    table.ForeignKey(
                        name: "fk_scores_beatmaps_beatmap_id",
                        column: x => x.beatmap_id,
                        principalTable: "beatmaps",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_scores_login_sessions_session_id",
                        column: x => x.session_id,
                        principalTable: "login_sessions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_scores_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_file_name",
                table: "beatmaps",
                column: "file_name");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_id",
                table: "beatmaps",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_mapset_id",
                table: "beatmaps",
                column: "mapset_id");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_md5",
                table: "beatmaps",
                column: "md5",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_mode",
                table: "beatmaps",
                column: "mode");

            migrationBuilder.CreateIndex(
                name: "ix_login_sessions_osu_version",
                table: "login_sessions",
                column: "osu_version");

            migrationBuilder.CreateIndex(
                name: "ix_login_sessions_user_id",
                table: "login_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_relationships_second_user_id",
                table: "relationships",
                column: "second_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_claims_role_id",
                table: "role_claims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "roles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_scores_beatmap_id",
                table: "scores",
                column: "beatmap_id");

            migrationBuilder.CreateIndex(
                name: "ix_scores_map_md5",
                table: "scores",
                column: "map_md5");

            migrationBuilder.CreateIndex(
                name: "ix_scores_map_md5_status_mode",
                table: "scores",
                columns: new[] { "map_md5", "status", "mode" });

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
                name: "ix_scores_session_id",
                table: "scores",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_scores_status",
                table: "scores",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_scores_user_id",
                table: "scores",
                column: "user_id");

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
                name: "ix_stats_user_id",
                table: "stats",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_claims_user_id",
                table: "user_claims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_logins_user_id",
                table: "user_logins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "users",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "users",
                column: "normalized_user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "profiles");

            migrationBuilder.DropTable(
                name: "relationships");

            migrationBuilder.DropTable(
                name: "role_claims");

            migrationBuilder.DropTable(
                name: "scores");

            migrationBuilder.DropTable(
                name: "stats");

            migrationBuilder.DropTable(
                name: "user_claims");

            migrationBuilder.DropTable(
                name: "user_logins");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "beatmaps");

            migrationBuilder.DropTable(
                name: "login_sessions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
