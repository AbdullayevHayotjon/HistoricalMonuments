using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Obidalar.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RasmUrl",
                table: "Obidalar");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Sharhlar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Obidalar",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Obidalar",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Obidalar",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ObidaMedialar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MediaUrl = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    ObidaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObidaMedialar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObidaMedialar_Obidalar_ObidaId",
                        column: x => x.ObidaId,
                        principalTable: "Obidalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Userlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Parol = table.Column<string>(type: "text", nullable: false),
                    Ism = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Userlar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObidaUser",
                columns: table => new
                {
                    LikedObidalarId = table.Column<int>(type: "integer", nullable: false),
                    LikedUsersId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObidaUser", x => new { x.LikedObidalarId, x.LikedUsersId });
                    table.ForeignKey(
                        name: "FK_ObidaUser_Obidalar_LikedObidalarId",
                        column: x => x.LikedObidalarId,
                        principalTable: "Obidalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObidaUser_Userlar_LikedUsersId",
                        column: x => x.LikedUsersId,
                        principalTable: "Userlar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sharhlar_UserId",
                table: "Sharhlar",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ObidaMedialar_ObidaId",
                table: "ObidaMedialar",
                column: "ObidaId");

            migrationBuilder.CreateIndex(
                name: "IX_ObidaUser_LikedUsersId",
                table: "ObidaUser",
                column: "LikedUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sharhlar_Userlar_UserId",
                table: "Sharhlar",
                column: "UserId",
                principalTable: "Userlar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sharhlar_Userlar_UserId",
                table: "Sharhlar");

            migrationBuilder.DropTable(
                name: "ObidaMedialar");

            migrationBuilder.DropTable(
                name: "ObidaUser");

            migrationBuilder.DropTable(
                name: "Userlar");

            migrationBuilder.DropIndex(
                name: "IX_Sharhlar_UserId",
                table: "Sharhlar");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sharhlar");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Obidalar");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Obidalar");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Obidalar");

            migrationBuilder.AddColumn<string>(
                name: "RasmUrl",
                table: "Obidalar",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
