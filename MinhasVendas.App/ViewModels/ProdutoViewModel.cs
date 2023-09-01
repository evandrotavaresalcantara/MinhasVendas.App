using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }

        public string? Nome { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoDeLista { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoBase { get; set; }
        public int EstoqueAtual { get; set; }

        /* Ef Relacionamento */
        ICollection<DetalheDeVendaViewModel>? DetalheDeVendas { get; set; }
        ICollection<DetalheDeCompraViewModel>? DetalheDeCompras { get; set; }
        ICollection<TransacaoDeEstoqueViewModel>? TransacaoDeEstoques { get; set; }
    }
}
