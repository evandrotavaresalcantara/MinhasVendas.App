using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces.Repositorio
{
    public interface IProdutoRepositorio : IRepositorio<Produto>
    {
        ListaPaginada<Produto> ObterProdutosPaginacaoLista(ProdutosParametros produtosParametros);
    }
}
