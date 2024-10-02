using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Vendas.Models.Enums;
using Vendas.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels
{
    public class OrdemDeVendaViewModel
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public StatusOrdemDeVenda StatusOrdemDeVenda { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }
        public DateTime? DataDePagamento { get; set; }
        public DateTime DataDeCriacao { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal ValorDeFrete { get; set; }

        /* Ef Relacionamento */
        public ICollection<DetalheDeVenda>? DetalheDeVendas { get; set; }
        public ICollection<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }

        public Cliente? Cliente { get; set; }
    }
}
