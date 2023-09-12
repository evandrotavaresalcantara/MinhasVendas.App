using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class FornecedorViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Documento { get; set; } = string.Empty;
        public TipoFornecedor TipoFornecedor { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;



        /* Ef Relacionamentos */
        public ICollection<OrdemDeCompraViewModel>? OrdemDeCompras { get; set; }
        public FornecedorEndereco? Endereco { get; set; }


    }
}
