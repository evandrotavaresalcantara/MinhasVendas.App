using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels
{
    public class ProdutoCategoriaViewModel
    {
        [Required(ErrorMessage ="Campo {0} é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Campo {0} é obrigatório")]
        public string Descricao { get; set; } = string.Empty;

        public ICollection<ProdutoViewModel>? Produtos { get; set; }
    }
}
