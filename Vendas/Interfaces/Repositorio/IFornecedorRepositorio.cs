using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces.Repositorio
{
    public interface IFornecedorRepositorio : IRepositorio<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEndereco(int id);
        Task<Fornecedor> ObterFornecedorProdutoEndereco(int id);
        ListaPaginada<Fornecedor> ObterFornecedoresaginacaoLista(FornecedoresParametros fornecedoresParametros);
    }
}
