using Vendas.Models;

namespace Vendas.Interfaces
{
    public interface ITransacaoDeEstoqueServico
    {
        Task<IEnumerable<SaldoDeEstoque>> ConsultaSaldoDeEstoque();
        Task<int> EstoqueAtualPorProduto(int id);
    }
}
