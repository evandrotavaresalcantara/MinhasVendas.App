using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio
{
    public class TransacaoDeEstoqueRepositorio : Repositorio<TransacaoDeEstoque>, ITransacaoDeEstoqueRepositorio
    {
        public TransacaoDeEstoqueRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }
    }
}
