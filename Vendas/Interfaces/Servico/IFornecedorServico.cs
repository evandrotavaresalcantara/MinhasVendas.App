using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces
{
    public interface IFornecedorServico
    {
        Task Adicionar(Fornecedor fornecedor);
        Task Atualizar(Fornecedor fornecedor);
        Task Remover(int id);
        Task<string> ObterFornecedores(FornecedoresParametros fornecedoresParametros);
    }
}
