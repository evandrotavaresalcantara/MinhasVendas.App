using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    /// <inheritdoc />
    public partial class DataCriacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataDeVenda",
                table: "OrdemDeVendas",
                newName: "DataDeCriacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataDeCriacao",
                table: "OrdemDeVendas",
                newName: "DataDeVenda");
        }
    }
}
