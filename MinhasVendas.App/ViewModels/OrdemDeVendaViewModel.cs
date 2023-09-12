using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
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
