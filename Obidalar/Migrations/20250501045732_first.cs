using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Obidalar.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Obidalar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nomi = table.Column<string>(type: "text", nullable: false),
                    Viloyat = table.Column<string>(type: "text", nullable: false),
                    Yil = table.Column<int>(type: "integer", nullable: false),
                    Tavsif = table.Column<string>(type: "text", nullable: false),
                    RasmUrl = table.Column<string>(type: "text", nullable: false),
                    XaritaUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obidalar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sharhlar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ObidaId = table.Column<int>(type: "integer", nullable: false),
                    Matn = table.Column<string>(type: "text", nullable: false),
                    Sana = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sharhlar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sharhlar_Obidalar_ObidaId",
                        column: x => x.ObidaId,
                        principalTable: "Obidalar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sharhlar_ObidaId",
                table: "Sharhlar",
                column: "ObidaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sharhlar");

            migrationBuilder.DropTable(
                name: "Obidalar");
        }
    }
}
