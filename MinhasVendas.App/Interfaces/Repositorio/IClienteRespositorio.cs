using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces.Repositorio
{
    public interface IClienteRespositorio : IRepositorio<Cliente> 
    {

        Task<Cliente> ObterClienteEndereco(int id);
        Task<Cliente> ObterClienteProdutoEndereco(int id);

    }
}
