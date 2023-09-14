using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces.Repositorio
{
    public interface IClienteRespositorio : IRepositorio<Cliente> 
    {

        Task<Cliente> ObterClienteEndereco(int id);
        Task<Cliente> ObterClienteProdutoEndereco(int id);
        ListaPaginada<Cliente> ObterClientesPaginacaoLista(ClientesParametros clientesParametros);

    }
}
