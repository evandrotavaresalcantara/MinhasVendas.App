using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces.Repositorio
{
    public interface IFornecedorRepositorio : IRepositorio<Fornecedor>
    {
        Task<Fornecedor> ObterFornecedorEndereco(int id);
        Task<Fornecedor> ObterFornecedorProdutoEndereco(int id);
    }
}
