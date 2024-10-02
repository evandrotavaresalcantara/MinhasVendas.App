using Vendas.Models;
using Vendas.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels
{
    public class FornecedorViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Documento { get; set; } = string.Empty;
        public int TipoFornecedor { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;



        /* Ef Relacionamentos */
        public ICollection<OrdemDeCompraViewModel>? OrdemDeCompras { get; set; }
        public FornecedorEnderecoViewModel? Endereco { get; set; }


    }
}
