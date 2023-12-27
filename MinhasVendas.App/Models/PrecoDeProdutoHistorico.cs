using System.Security.Principal;

namespace MinhasVendas.App.Models
{
    public class PrecoDeProdutoHistorico : Entidade
    {
        public int ProdutoId { get; set; }

        public decimal PrecoDeCusto { get; set; }
        public decimal MarkUp { get; set; }
        public decimal PrecoDeVenda { get; set; }

        public int EstoqueAtual { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public Produto? Produto { get; set; }
    }
}
