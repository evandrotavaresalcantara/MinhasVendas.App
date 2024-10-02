using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces.Repositorio;

public interface IOrdemDeVendaRepositorio : IRepositorio<OrdemDeVenda>
{
    ListaPaginada<OrdemDeVenda> ObterOrdemDeVendasPaginacaoLista(OrdemDeVendasParametros ordemDeVendasParametros);
}
