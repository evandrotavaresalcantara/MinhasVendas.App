using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    /// <inheritdoc />
    public partial class historicodepreco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrecoDeProdutoHistoricos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProdutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecoDeCusto = table.Column<decimal>(type: "TEXT", nullable: false),
                    MarkUp = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrecoDeVenda = table.Column<decimal>(type: "TEXT", nullable: false),
                    EstoqueAtual = table.Column<int>(type: "INTEGER", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrecoDeProdutoHistoricos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrecoDeProdutoHistoricos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrecoDeProdutoHistoricos_ProdutoId",
                table: "PrecoDeProdutoHistoricos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrecoDeProdutoHistoricos");
        }
    }
}
