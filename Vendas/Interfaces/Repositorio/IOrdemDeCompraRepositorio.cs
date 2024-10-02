using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Interfaces.Repositorio
{
    public interface IOrdemDeCompraRepositorio : IRepositorio<OrdemDeCompra>
    {
        ListaPaginada<OrdemDeCompra> ObterOrdemDecomprasPaginacaoLista(OrdemDeComprasParametros ordemDeComprasParametros);
    }
}
