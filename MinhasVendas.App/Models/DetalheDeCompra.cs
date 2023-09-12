using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinhasVendas.App.Models
{
    public class DetalheDeCompra : Entidade
    {

        public int ProdutoId { get; set; }
        public int OrdemDeCompraId { get; set; }
        public int TransacaoDeEstoqueId { get; set; }
        public int Quantidade { get; set; }

        public decimal CustoUnitario { get; set; }
        
        public DateTime? DataDeRecebimento {  get; set; }
        public bool RegistradoTransacaoDeEstoque { get; set; }

        /* EF Relacionamento */
        public Produto? Produto { get; set; }
        public OrdemDeCompra? OrdemDeCompra { get; set; }

    }
}
