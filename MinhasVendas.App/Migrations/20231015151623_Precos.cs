using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    /// <inheritdoc />
    public partial class Precos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrecoDeLista",
                table: "Produtos",
                newName: "PrecoDeVenda");

            migrationBuilder.RenameColumn(
                name: "PrecoBase",
                table: "Produtos",
                newName: "PrecoDeCusto");

            migrationBuilder.AddColumn<decimal>(
                name: "MarkUp",
                table: "Produtos",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkUp",
                table: "Produtos");

            migrationBuilder.RenameColumn(
                name: "PrecoDeVenda",
                table: "Produtos",
                newName: "PrecoDeLista");

            migrationBuilder.RenameColumn(
                name: "PrecoDeCusto",
                table: "Produtos",
                newName: "PrecoBase");
        }
    }
}
