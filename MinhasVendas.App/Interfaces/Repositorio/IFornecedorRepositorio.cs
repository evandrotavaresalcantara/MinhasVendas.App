using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces.Repositorio
{
    public interface IFornecedorRepositorio : IRepositorio<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEndereco(int id);
        Task<Fornecedor> ObterFornecedorProdutoEndereco(int id);
        ListaPaginada<Fornecedor> ObterFornecedoresaginacaoLista(FornecedoresParametros fornecedoresParametros);
    }
}
