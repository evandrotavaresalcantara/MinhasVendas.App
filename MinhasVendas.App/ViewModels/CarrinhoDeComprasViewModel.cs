using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class CarrinhoDeComprasViewModel
    {
        public Produto? Produto { get; set; }
        public DetalheDeCompra? DetalheDeCompra { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalProduto { get; set; }

        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalCompra { get; set; }

        public int TotalItens { get; set; }
    }
}
