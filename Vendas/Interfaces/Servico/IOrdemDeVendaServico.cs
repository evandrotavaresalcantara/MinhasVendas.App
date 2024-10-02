using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces
{
    public interface IOrdemDeVendaServico
    {
        Task Adicionar(OrdemDeVenda ordemDeVenda);
        Task Atualizar(OrdemDeVenda ordemDeVenda);
        Task Remover(int id);
        Task FinalizarVenda(OrdemDeVenda ordemDeVenda);
        Task FinalizarVendaView(int id);
        Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(int id);
        Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalheDeVenda(int id);
        Task<IEnumerable<OrdemDeVenda>> ConsultaOrdemDevendaCliente();
        Task<OrdemDeVenda> ConsultaOrdemDeVenda(int id);

        Task InserirFrete(OrdemDeVenda ordemDeVenda);
        Task InserirFrete(int id);
        Task<string> ObterOrdemVendas(OrdemDeVendasParametros ordemDeVendasParametros);
    }
}
