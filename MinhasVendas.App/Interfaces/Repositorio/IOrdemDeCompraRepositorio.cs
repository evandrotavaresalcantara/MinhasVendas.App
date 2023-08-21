using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces.Repositorio
{
    public interface IOrdemDeCompraRepositorio : IRepositorio<OrdemDeCompra>
    {
        ListaPaginada<OrdemDeCompra> ObterOrdemDecomprasPaginacaoLista(OrdemDeComprasParametros ordemDeComprasParametros);
    }
}
