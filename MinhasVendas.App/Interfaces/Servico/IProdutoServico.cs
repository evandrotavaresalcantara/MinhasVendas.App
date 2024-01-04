using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces
{
    public interface IProdutoServico
    {
        Task Adicionar(Produto produto);
        Task Atualizar(Produto produto);
        Task Remover(int id);
        Task<IEnumerable<Produto>> ConsultaProdutos();

        Task AtualizarPreco(int id, decimal precoDeCusto, decimal markup, decimal precoDeVenda);
        Task<string> ObterProdutos(ProdutosParametros produtosParametros);
    }
}
