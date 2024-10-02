using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio
{
    public class DetalheDeCompraRepositorio : Repositorio<DetalheDeCompra>, IDetalheDeCompraRepositorio
    {
        public DetalheDeCompraRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
