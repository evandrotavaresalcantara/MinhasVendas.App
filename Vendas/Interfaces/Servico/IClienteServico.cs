using Vendas.Models;
using Vendas.Paginacao;
using Vendas.Servicos;

namespace Vendas.Interfaces.Servico
{
    public interface IClienteServico
    {
        Task Adicionar(Cliente cliente);
        Task Atualizar(Cliente cliente);
        Task Remover(int id);

        Task<string> ObterClientes(ClientesParametros clientesParametros);

    }
}
