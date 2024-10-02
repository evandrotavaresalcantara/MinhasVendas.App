using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces.Repositorio
{
    public interface IClienteRespositorio : IRepositorio<Cliente> 
    {

        Task<Cliente> ObterClienteEndereco(int id);
        Task<Cliente> ObterClienteProdutoEndereco(int id);
        ListaPaginada<Cliente> ObterClientesPaginacaoLista(ClientesParametros clientesParametros);

    }
}
