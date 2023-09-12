using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.Models
{
    public class OrdemDeVenda : Entidade
    {
        public int ClienteId { get; set; }
       
        public StatusOrdemDeVenda StatusOrdemDeVenda { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }
        public DateTime? DataDePagamento { get; set; }
        public DateTime DataDeCriacao { get; set; } 

        public Decimal ValorDeFrete { get; set; }

        /* Ef Relacionamento */
        public ICollection<DetalheDeVenda>? DetalheDeVendas { get; set; }
        public ICollection<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }

        public Cliente? Cliente { get; set; }
    }
}
