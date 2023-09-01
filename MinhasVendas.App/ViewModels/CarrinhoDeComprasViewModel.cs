using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class CarrinhoDeComprasViewModel
    {
        public ProdutoViewModel? Produto { get; set; }
        public DetalheDeCompraViewModel? DetalheDeCompraViewModel { get; set; }
        public OrdemDeCompraViewModel? OrdemDeCompraViewModel { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalProduto { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalProdutos { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCompra { get; set; }

        public int TotalItens { get; set; }
    }
}
