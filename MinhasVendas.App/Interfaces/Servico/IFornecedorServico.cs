using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces
{
    public interface IFornecedorServico
    {
        Task Adicionar(Fornecedor fornecedor);
        Task Atualizar(Fornecedor fornecedor);
        Task Remover(int id);
        Task<string> ObterFornecedores(FornecedoresParametros fornecedoresParametros);
    }
}
