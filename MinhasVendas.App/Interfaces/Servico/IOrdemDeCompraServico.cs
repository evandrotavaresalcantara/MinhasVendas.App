using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Interfaces
{
    public interface IOrdemDeCompraServico
    {
        Task Adicionar(OrdemDeCompra ordemDeCompra);
        Task Atualizar(OrdemDeCompra ordemDeCompra);
        Task Remover(int id);

        Task FinalizarCompra(OrdemDeCompra ordemDeCompra);
        Task FinalizarCompraView(int id);
        
        Task SolicitarAprovacao(OrdemDeCompra ordemDeCompra);
        Task SolicitarAprovacao(int id);

        Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(int id);
        Task<IEnumerable<OrdemDeCompra>> ConsultaOrdemDeCompraFornecedor();
        Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompra(int id);
        Task<OrdemDeCompra> ConsultaOrdemDeCompra(int id);

        Task<string> ObterOrdemCompras(OrdemDeComprasParametros ordemDeComprasParametros);

    }
}
