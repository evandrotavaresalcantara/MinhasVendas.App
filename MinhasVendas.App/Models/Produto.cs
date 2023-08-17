using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.Models
{
    public class Produto : Entidade
    {
        //public int Id { get; set; }

        public string? Nome { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoDeLista { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoBase { get; set; }
        public int EstoqueAtual { get; set; }

        /* Ef Relacionamento */
        ICollection<DetalheDeVenda>? DetalheDeVendas { get; set; }
        ICollection<DetalheDeCompra>? DetalheDeCompras { get; set; }
        ICollection<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }
    }
}
