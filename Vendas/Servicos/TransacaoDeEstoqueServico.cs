using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Models.Enums;
using System.Collections;

namespace Vendas.Servicos
{
    public class TransacaoDeEstoqueServico : BaseServico, ITransacaoDeEstoqueServico
    {
        private readonly VendasAppContext _minhasVendasAppContext;
        private readonly IProdutoRepositorio _produtoRepositorio;
        public TransacaoDeEstoqueServico(INotificador notificador,
                                         IProdutoRepositorio produtoRepositorio,
                                         VendasAppContext minhasVendasAppContext) : base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IEnumerable<SaldoDeEstoque>> ConsultaSaldoDeEstoque()
        {

            var qtdCompraEVenda =
                       from produto in _minhasVendasAppContext.TransacaoDeEstoques.AsNoTracking()
                       group produto by produto.ProdutoId into produtoGroup
                       select new
                       {
                           produtoGroup.Key,
                           totalProdutoComprado = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra).Sum(p => p.Quantidade),
                           totalProdutoVendido = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Venda).Sum(p => p.Quantidade)
                       };

            var produtos = await _minhasVendasAppContext.Produtos.AsNoTracking().ToListAsync();

            foreach (var produto in qtdCompraEVenda)
            {
                var item = produtos.Find(p => p.Id == produto.Key);
                item.EstoqueAtual = produto.totalProdutoComprado - produto.totalProdutoVendido;
            }

            var listaDeProdutos = (from c in produtos
                                   select new SaldoDeEstoque
                                   {
                                       Id = c.Id,
                                       NomeProduto = c.Nome,
                                       Preco = (c.PrecoDeCusto * (c.PrecoDeCusto * c.MarkUp)) + c.PrecoDeCusto,
                                       EstoqueAtual = c.EstoqueAtual,
                                       ProdutoCompleto = c.Nome + " | " + "Valor: R$ " + " " + ((c.PrecoDeCusto * (c.PrecoDeCusto * c.MarkUp)) + c.PrecoDeCusto) + " | " + c.EstoqueAtual
                                   });


            return listaDeProdutos;

        }
        
        public async Task<int> EstoqueAtualPorProduto(int id)
        {

            var qtdCompraEVenda = from produto in _minhasVendasAppContext.TransacaoDeEstoques
                                  where produto.ProdutoId == id
                                  group produto by produto.ProdutoId into produtoGroup
                                  select new
                                  {
                                      produtoGroup.Key,
                                      totalProdutoComprado = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra).Sum(p => p.Quantidade),
                                      totalProdutoVendido = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Venda).Sum(p => p.Quantidade)
                                  };

            var produtoDB = await _produtoRepositorio.Obter().FirstOrDefaultAsync(p => p.Id == id); // Poderia verificar se é null

            var estoqueAtual = qtdCompraEVenda.Select(q => q.totalProdutoComprado - q.totalProdutoVendido).FirstOrDefault();

            return estoqueAtual;


        }


    }
}


