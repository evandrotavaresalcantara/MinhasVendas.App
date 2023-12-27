using MinhasVendas.App.Models;

namespace MinhasVendas.App.Interfaces
{
    public interface ITransacaoDeEstoqueServico
    {
        Task<IEnumerable<SaldoDeEstoque>> ConsultaSaldoDeEstoque();
        Task<int> EstoqueAtualPorProduto(int id);
    }
}
