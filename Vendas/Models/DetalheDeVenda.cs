﻿using System.ComponentModel.DataAnnotations;

namespace Vendas.Models
{
    public class DetalheDeVenda : Entidade
    {
        public int OrdemDeVendaId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Desconto { get; set; }
        public bool RegistroTransacaoDeEstoque { get; set; }
        public int TransacaoDeEstoqueId { get; set; }

        /* Ef Relacionamento*/
        public OrdemDeVenda? OrdemDeVenda { get; set; }
        public Produto? Produto { get; set; }

    }
}
