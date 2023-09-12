using MinhasVendas.App.Models;

namespace MinhasVendas.App.ViewModels
{
    public class CarrinhoDeVendasViewModel
    {
        public ProdutoViewModel? ProdutoViewModel { get; set; }
        public DetalheDeVendaViewModel? DetalheDeVendaViewModel { get; set; }
        public OrdemDeVendaViewModel? OrdemDeVendaViewModel { get; set; }

        public int TotalItens { get; set; }
        public decimal TotalProdutos { get; set; }

        public decimal TotalVenda { get; set; }

    }
}
