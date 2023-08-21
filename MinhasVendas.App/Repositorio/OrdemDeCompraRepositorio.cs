using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Repositorio
{
    public class OrdemDeCompraRepositorio : Repositorio<OrdemDeCompra>, IOrdemDeCompraRepositorio
    {
        public OrdemDeCompraRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }


        public ListaPaginada<OrdemDeCompra> ObterOrdemDecomprasPaginacaoLista(OrdemDeComprasParametros ordemDeComprasParametros)
        {
            return ListaPaginada<OrdemDeCompra>.ParaListaPaginada(Obter().OrderBy(on => on.DataDeCriacao),
                    ordemDeComprasParametros.NumeroDePagina,
                    ordemDeComprasParametros.TamanhoDePagina);
        }
    }
}
