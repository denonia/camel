using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Camel.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddStatsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "ix_stats_user_id",
                table: "stats",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stats");
        }
    }
}
