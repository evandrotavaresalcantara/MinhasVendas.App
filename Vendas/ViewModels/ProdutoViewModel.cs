using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Vendas.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Vendas.Extensoes;

namespace Vendas.ViewModels
{
    public class ProdutoViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public int ProdutoCategoriaId { get; set; }

        [Required(ErrorMessage ="# Campo {0} obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "# Campo {0} obrigatório")]
        public string? Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "# Campo {0} obrigatório")]
        public string? Descricao { get; set; } = string.Empty;
        public IFormFile? ImagemUpload { get; set; } 
        public string? Imagem { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }

        public decimal? PrecoDeCusto { get; set; }

        public decimal? MarkUp { get; set; }

        public decimal? PrecoDeVenda { get; set; }

        public string? Tamanho { get; set; }

        public int EstoqueAtual { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (PrecoDeVenda == null || PrecoDeVenda == 0 || PrecoDeCusto == null || PrecoDeCusto == 0)
            {
                yield return new ValidationResult(
                    "# Preço de Venda e Custo precisar ser preenchido, não pode ser zero.",
                    new[] { nameof(PrecoDeVenda) });
            }
            else
            {
                if (PrecoDeVenda < PrecoDeCusto)
                {
                    yield return new ValidationResult(
                        "# O Preço de Venda não pode ser menor que o Preço de Custo",
                        new[] { nameof(PrecoDeVenda) });
                }
            }

        }

        /* Ef Relacionamento */
        public ProdutoCategoria? ProdutoCategoria { get; set; }
        public ICollection<DetalheDeVendaViewModel>? DetalheDeVendas { get; set; }
        public ICollection<DetalheDeCompraViewModel>? DetalheDeCompras { get; set; }
    }

  
}




