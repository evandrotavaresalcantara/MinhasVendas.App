using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio
{
    public class DetalheDeVendaRepositorio : Repositorio<DetalheDeVenda>, IDetalheDeVendaRepositorio
    {
        public DetalheDeVendaRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
