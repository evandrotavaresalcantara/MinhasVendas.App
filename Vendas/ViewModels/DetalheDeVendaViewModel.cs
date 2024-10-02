using Vendas.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels
{
    public class DetalheDeVendaViewModel
    {
        [Key]
        public int Id { get; set; }

        public int OrdemDeVendaId { get; set; }
        public int ProdutoId { get; set; }


        [DisplayName("Quantidade")]
        [Required(ErrorMessage = "# O campo {0} é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "# Valor Inválido")]
        public int Quantidade { get; set; }


        public decimal PrecoUnitario { get; set; }


        [Required(ErrorMessage = "# O campo {0} é obrigatório")]
        [Range(0, 20, ErrorMessage = "# Valor inválido")]
        public decimal Desconto { get; set; }
        public bool RegistroTransacaoDeEstoque { get; set; }
        public int TransacaoDeEstoqueId { get; set; }

        /* Ef Relacionamento*/
        public OrdemDeVenda? OrdemDeVenda { get; set; }
        public Produto? Produto { get; set; }
    }
}
