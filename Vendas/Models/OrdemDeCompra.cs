using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Vendas.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Vendas.Models
{
    public class OrdemDeCompra : Entidade
    {

        public int FornecedorId { get; set; }
        public DateTime DataDeCriacao { get; set; }
        public StatusOrdemDeCompra StatusOrdemDeCompra { get; set; }
        public FormaDePagamento FormaDePagamento { get; set; }

        public Decimal ValorDeFrete { get; set; }


        /* Ef Relacionamento */
        public Fornecedor? Fornecedor { get; set; }
        public ICollection<DetalheDeCompra>? DetalheDeCompras { get; set; }
       
    }
}
