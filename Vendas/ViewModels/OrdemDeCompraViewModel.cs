using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Vendas.Models.Enums;
using Vendas.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels
{
    public class OrdemDeCompraViewModel
    {
        [Key]
        public int Id { get; set; }

        public int FornecedorId { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public StatusOrdemDeCompra StatusOrdemDeCompra { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal ValorDeFrete { get; set; }


        public FornecedorViewModel? Fornecedor { get; set; }
        public ICollection<DetalheDeCompraViewModel>? DetalheDeCompras { get; set; }
    }
}
