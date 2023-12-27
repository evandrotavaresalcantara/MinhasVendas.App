using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class ProdutoViewModel
    {
        public int Id { get; set; }
        public int ProdutoCategoriaId { get; set; }

        public string? Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public IFormFile? ImagemUpload { get; set; } 
        public string Imagem { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataDeCadastro { get; set; }

        [Required(ErrorMessage = "# O campo {0} é obrigatório")]
        //[Range(0, double.MaxValue, ErrorMessage = "# Valor inválido")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoDeCusto { get; set; }

        [Required(ErrorMessage = "# O campo {0} é obrigatório")]
        //[Range(-101, double.MaxValue, ErrorMessage = "# Valor inválido")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal MarkUp { get; set; }

        [Required(ErrorMessage = "# O campo {0} é obrigatório")]
        //[Range(0, double.MaxValue, ErrorMessage = "# Valor inválido")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecoDeVenda { get; set; }




        public int EstoqueAtual { get; set; }

        /* Ef Relacionamento */
        public ProdutoCategoria? ProdutoCategoria { get; set; }
        public ICollection<DetalheDeVendaViewModel>? DetalheDeVendas { get; set; }
        public ICollection<DetalheDeCompraViewModel>? DetalheDeCompras { get; set; }
    }
}




