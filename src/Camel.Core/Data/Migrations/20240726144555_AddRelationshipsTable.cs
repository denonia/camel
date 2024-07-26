using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Camel.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationshipsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_beatmaps",
                table: "beatmaps");

            migrationBuilder.AddColumn<int>(
                name: "session_id",
                table: "scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "beatmaps",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_beatmaps",
                table: "beatmaps",
                column: "md5");

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

            migrationBuilder.CreateIndex(
                name: "ix_scores_session_id",
                table: "scores",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_id",
                table: "beatmaps",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_beatmaps_mapset_id",
                table: "beatmaps",
                column: "mapset_id");

            migrationBuilder.CreateIndex(
                name: "ix_relationships_second_user_id",
                table: "relationships",
                column: "second_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_scores_beatmaps_map_md5",
                table: "scores",
                column: "map_md5",
                principalTable: "beatmaps",
                principalColumn: "md5",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_scores_login_sessions_session_id",
                table: "scores",
                column: "session_id",
                principalTable: "login_sessions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_scores_beatmaps_map_md5",
                table: "scores");

            migrationBuilder.DropForeignKey(
                name: "fk_scores_login_sessions_session_id",
                table: "scores");

            migrationBuilder.DropTable(
                name: "relationships");

            migrationBuilder.DropIndex(
                name: "ix_scores_session_id",
                table: "scores");

            migrationBuilder.DropPrimaryKey(
                name: "pk_beatmaps",
                table: "beatmaps");

            migrationBuilder.DropIndex(
                name: "ix_beatmaps_id",
                table: "beatmaps");

            migrationBuilder.DropIndex(
                name: "ix_beatmaps_mapset_id",
                table: "beatmaps");

            migrationBuilder.DropColumn(
                name: "session_id",
                table: "scores");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "beatmaps",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_beatmaps",
                table: "beatmaps",
                column: "id");
        }
    }
}
