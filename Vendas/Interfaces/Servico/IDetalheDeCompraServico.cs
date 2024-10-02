using Vendas.Models;

namespace Vendas.Interfaces
{
    public interface IDetalheDeCompraServico
    {
        Task Adicionar(DetalheDeCompra detalheDeCompra);
        Task Atualizar(DetalheDeCompra detalheDeCompra);
        Task RemoverStatus(int id);
        Task Remover(int id);
     
        Task InserirProdutoStatus (int id);
        
        Task ReceberProduto(DetalheDeCompra detalheDeCompra);
        Task ReceberProduto(int id);
        
        Task<DetalheDeCompra> Consulta(int id);
        Task<DetalheDeCompra> ConsultaDetalheDeCompraProdutoOrdemDeCompra(int id);
              
    }
}
