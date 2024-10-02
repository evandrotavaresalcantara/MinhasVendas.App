using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces
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
