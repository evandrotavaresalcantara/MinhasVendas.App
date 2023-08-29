using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces.Repositorio;

public interface IOrdemDeVendaRepositorio : IRepositorio<OrdemDeVenda>
{
    ListaPaginada<OrdemDeVenda> ObterOrdemDeVendasPaginacaoLista(OrdemDeVendasParametros ordemDeVendasParametros);
}
