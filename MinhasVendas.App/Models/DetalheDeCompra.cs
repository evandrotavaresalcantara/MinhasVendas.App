using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhasVendas.App.Models
{
    public class DetalheDeCompra : Entidade
    {
        //public int Id { get; set; }

        public int ProdutoId { get; set; }
        public int OrdemDeCompraId { get; set; }
        public int TransacaoDeEstoqueId { get; set; }
        [Range(1, 1000, ErrorMessage = "Valor Inválido")]
        public int Quantidade { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CustoUnitario { get; set; }
        public DateTime? DataDeRecebimento {  get; set; }
        public bool RegistradoTransacaoDeEstoque { get; set; }

        /* EF Relacionamento */
        public Produto? Produto { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }

    }
}
