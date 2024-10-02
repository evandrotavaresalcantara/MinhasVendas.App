using Vendas.Models;
using System.ComponentModel.DataAnnotations;

namespace Vendas.ViewModels
{
    public class ClienteViewModel
    {
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string SobreNome { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string Celular { get; set; } = string.Empty;
        public string WhatsApp { get; set; } = string.Empty;
        public string Instagram { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public ClienteEnderecoViewModel? Endereco { get; set; }
        public ICollection<OrdemDeVendaViewModel>? OrdemDeVendas { get; set; }
    }
}
