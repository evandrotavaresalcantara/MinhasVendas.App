using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class DetalheDeCompraViewModel
    {
        [Key]
        public int Id { get; set; }

        public int ProdutoId { get; set; }
        public int OrdemDeCompraId { get; set; }
        public int TransacaoDeEstoqueId { get; set; }
        [Range(1, 1000, ErrorMessage = "Valor Inválido")]
        public int Quantidade { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal CustoUnitario { get; set; }

        [Display(Name = "Data de Recebimento")]
        [DataType(DataType.Date)]
        public DateTime? DataDeRecebimento { get; set; }
        public bool RegistradoTransacaoDeEstoque { get; set; }

        public ProdutoViewModel? Produto { get; set; }
        public OrdemDeCompraViewModel? OrdemDeCompra { get; set; }
    }
}
