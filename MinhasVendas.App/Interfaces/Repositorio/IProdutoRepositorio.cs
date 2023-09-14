using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces.Repositorio
{
    public interface IProdutoRepositorio : IRepositorio<Produto>
    {
        ListaPaginada<Produto> ObterProdutosPaginacaoLista(ProdutosParametros produtosParametros);
    }
}
