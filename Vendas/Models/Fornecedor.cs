using Vendas.Models.Enums;

namespace Vendas.Models
{
    public class Fornecedor : Entidade
    {
        public string Documento { get; set; } = string.Empty;
        public TipoFornecedor TipoFornecedor { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;



        /* Ef Relacionamentos */
        public ICollection<OrdemDeCompra>? OrdemDeCompras { get; set; }
        public FornecedorEndereco? Endereco { get; set; }

    }
}
