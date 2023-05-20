using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KosarkaskaLiga2019.Migrations
{
    public partial class Druga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grad",
                columns: table => new
                {
                    GradId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NazivGrada = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grad", x => x.GradId);
                });

            migrationBuilder.CreateTable(
                name: "Tim",
                columns: table => new
                {
                    TimId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(maxLength: 30, nullable: false),
                    Slika = table.Column<byte[]>(nullable: true),
                    TipSLike = table.Column<string>(maxLength: 30, nullable: true),
                    GradId = table.Column<int>(nullable: true),
                    BrojBodova = table.Column<int>(nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tim", x => x.TimId);
                    table.ForeignKey(
                        name: "FK__Tim__GradId__2CF2ADDF",
                        column: x => x.GradId,
                        principalTable: "Grad",
                        principalColumn: "GradId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Utakmica",
                columns: table => new
                {
                    UtakmicaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DomacinId = table.Column<int>(nullable: false),
                    GostId = table.Column<int>(nullable: false),
                    Rezultat = table.Column<string>(unicode: false, maxLength: 7, nullable: true),
                    Kolo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utakmica", x => x.UtakmicaId);
                    table.ForeignKey(
                        name: "FK__Utakmica__Domaci__2DE6D218",
                        column: x => x.DomacinId,
                        principalTable: "Tim",
                        principalColumn: "TimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Utakmica__GostId__2EDAF651",
                        column: x => x.GostId,
                        principalTable: "Tim",
                        principalColumn: "TimId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tim_GradId",
                table: "Tim",
                column: "GradId");

            migrationBuilder.CreateIndex(
                name: "IX_Utakmica_DomacinId",
                table: "Utakmica",
                column: "DomacinId");

            migrationBuilder.CreateIndex(
                name: "IX_Utakmica_GostId",
                table: "Utakmica",
                column: "GostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Utakmica");

            migrationBuilder.DropTable(
                name: "Tim");

            migrationBuilder.DropTable(
                name: "Grad");
        }
    }
}
