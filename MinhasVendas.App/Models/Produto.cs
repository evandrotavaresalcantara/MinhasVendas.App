using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.Models
{
    public class Produto : Entidade
    {
        public int ProdutoCategoriaId { get; set; }

        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public decimal PrecoBase { get; set; }
        public decimal PrecoDeLista { get; set; }
        public string? Imagem { get; set; }

        public int EstoqueAtual { get; set; }

        /* Ef Relacionamento */
        public ProdutoCategoria? ProdutoCategoria { get; set; }
        public ICollection<DetalheDeVenda>? DetalheDeVendas { get; set; }
        public ICollection<DetalheDeCompra>? DetalheDeCompras { get; set; }
        public ICollection<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }
    }
}
