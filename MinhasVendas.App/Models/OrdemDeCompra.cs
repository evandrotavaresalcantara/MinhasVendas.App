using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.Models
{
    public class OrdemDeCompra : Entidade
    {
        //public int Id { get; set; }

        public int FornecedorId { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public StatusOrdemDeCompra StatusOrdemDeCompra { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public Decimal ValorDeFrete { get; set; }


        /* Ef Relacionamento */
        public Fornecedor? Fornecedor { get; set; }
        public ICollection<DetalheDeCompra>? DetalheDeCompras { get; set; }
       
    }
}
