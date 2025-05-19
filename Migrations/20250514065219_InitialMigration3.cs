using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EspnBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFLGames_Seasons_SeasonId",
                table: "NFLGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeamHistories_Players_PlayerId",
                table: "PlayerTeamHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeamHistories_Teams_TeamId",
                table: "PlayerTeamHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTeamHistories",
                table: "PlayerTeamHistories");

            migrationBuilder.DropColumn(
                name: "AwayTeam",
                table: "NFLGames");

            migrationBuilder.DropColumn(
                name: "HomeTeam",
                table: "NFLGames");

            migrationBuilder.RenameTable(
                name: "PlayerTeamHistories",
                newName: "PlayerTeamHistory");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTeamHistories_TeamId",
                table: "PlayerTeamHistory",
                newName: "IX_PlayerTeamHistory_TeamId");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "NFLGames",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamId",
                table: "NFLGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HomeTeamId",
                table: "NFLGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTeamHistory",
                table: "PlayerTeamHistory",
                columns: new[] { "PlayerId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_NFLGames_AwayTeamId",
                table: "NFLGames",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_NFLGames_HomeTeamId",
                table: "NFLGames",
                column: "HomeTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_NFLGames_Seasons_SeasonId",
                table: "NFLGames",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NFLGames_Teams_AwayTeamId",
                table: "NFLGames",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NFLGames_Teams_HomeTeamId",
                table: "NFLGames",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeamHistory_Players_PlayerId",
                table: "PlayerTeamHistory",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeamHistory_Teams_TeamId",
                table: "PlayerTeamHistory",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NFLGames_Seasons_SeasonId",
                table: "NFLGames");

            migrationBuilder.DropForeignKey(
                name: "FK_NFLGames_Teams_AwayTeamId",
                table: "NFLGames");

            migrationBuilder.DropForeignKey(
                name: "FK_NFLGames_Teams_HomeTeamId",
                table: "NFLGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeamHistory_Players_PlayerId",
                table: "PlayerTeamHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeamHistory_Teams_TeamId",
                table: "PlayerTeamHistory");

            migrationBuilder.DropIndex(
                name: "IX_NFLGames_AwayTeamId",
                table: "NFLGames");

            migrationBuilder.DropIndex(
                name: "IX_NFLGames_HomeTeamId",
                table: "NFLGames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTeamHistory",
                table: "PlayerTeamHistory");

            migrationBuilder.DropColumn(
                name: "AwayTeamId",
                table: "NFLGames");

            migrationBuilder.DropColumn(
                name: "HomeTeamId",
                table: "NFLGames");

            migrationBuilder.RenameTable(
                name: "PlayerTeamHistory",
                newName: "PlayerTeamHistories");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTeamHistory_TeamId",
                table: "PlayerTeamHistories",
                newName: "IX_PlayerTeamHistories_TeamId");

            migrationBuilder.AlterColumn<int>(
                name: "SeasonId",
                table: "NFLGames",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "AwayTeam",
                table: "NFLGames",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HomeTeam",
                table: "NFLGames",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTeamHistories",
                table: "PlayerTeamHistories",
                columns: new[] { "PlayerId", "TeamId" });

            migrationBuilder.AddForeignKey(
                name: "FK_NFLGames_Seasons_SeasonId",
                table: "NFLGames",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeamHistories_Players_PlayerId",
                table: "PlayerTeamHistories",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeamHistories_Teams_TeamId",
                table: "PlayerTeamHistories",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
