namespace Vendas.Models
{
    public class Cliente : Entidade
    {

        public string Nome { get; set; }   = string.Empty;
        public string SobreNome { get; set; } = string.Empty;
        public string? Documento { get; set; }
        DateOnly DataNascimento { get; set; }
        public string Celular { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


        /* Ef Relacionamento */
        public ICollection<OrdemDeVenda>? OrdemDeVendas { get; set; }
        public ClienteEndereco? Endereco { get; set; } 
     
    }
}
