using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Models.Enums;
using Vendas.Paginacao;
using Vendas.Repositorio;
using Newtonsoft.Json;
using System.Globalization;

namespace Vendas.Servicos
{
    public class ProdutoServico : BaseServico, IProdutoServico
    {
        private readonly VendasAppContext _minhasVendasAppContext;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IPrecoDeProdutoHistoricoRepositorio _precoDeProdutoHistoricoRepositorio;
        private readonly ITransacaoDeEstoqueServico _transacaoDeEstoqueServico;

        public ProdutoServico(VendasAppContext minhasVendasAppContext,
                              IProdutoRepositorio produtoRepositorio,
                              IPrecoDeProdutoHistoricoRepositorio precoDeProdutoHistoricoRepositorio,
                              ITransacaoDeEstoqueServico transacaoDeEstoqueServico,
                              INotificador notificador) : base(notificador)
        {
            _produtoRepositorio = produtoRepositorio;
            _minhasVendasAppContext = minhasVendasAppContext;
            _precoDeProdutoHistoricoRepositorio = precoDeProdutoHistoricoRepositorio;
            _transacaoDeEstoqueServico = transacaoDeEstoqueServico;
        }

        public async Task Adicionar(Produto produto)
        {

            if (_minhasVendasAppContext.Produtos.ToListAsync().Result.Any(p => p.Nome == produto.Nome))
            {
                Notificar("Já existe um produto com este nome informado.");
                return;
            }

            if (produto.PrecoDeVenda < produto.PrecoDeCusto)
            {
                Notificar("Preço de venda não pode ser menor que o preço de custo");
                return;
            }

            produto.DataDeCadastro = DateTime.Now.ToUniversalTime();
            _minhasVendasAppContext.Add(produto);
            await _minhasVendasAppContext.SaveChangesAsync();
        }

        public async Task Atualizar(Produto produto)
        {
            var produtoDB = await _produtoRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(p => p.Id == produto.Id);

            if (produtoDB == null)
            {
                Notificar("Produto não encontrado");
            }

            if (_produtoRepositorio.Obter().AsNoTracking().ToListAsync().Result.Any(p => p.Nome == produto.Nome && p.Id != produto.Id))
            {
                Notificar("Já existe um produto com este nome informado.");
                return;
            }

            if (produto.PrecoDeVenda < produto.PrecoDeCusto)
            {
                Notificar("Preço de venda não pode ser menor que o preço de custo");
                return;
            }

            await _produtoRepositorio.Atualizar(produto);

        }

        public async Task<IEnumerable<Produto>> ConsultaProdutos()
        {
            var produtos = await _minhasVendasAppContext.Produtos.AsNoTracking().ToListAsync();

            return produtos;
        }

        public async Task<string> ObterProdutos(ProdutosParametros produtosParametros)
        {
            IQueryable<Produto> produtosQuery = _produtoRepositorio.Obter().Include(c => c.ProdutoCategoria);

            if (!string.IsNullOrWhiteSpace(produtosParametros.search))
            {
                produtosParametros.search = produtosParametros.search.ToLower();

                produtosQuery = produtosQuery.Where(c =>
                    c.Nome.ToLower().Contains(produtosParametros.search) ||
                    c.Codigo.ToLower().Contains(produtosParametros.search)
                );
            }

            var filtro = produtosParametros.Filtro;

            if (filtro == 50 || filtro == 100 || filtro == 150)
            {

                produtosQuery = produtosQuery.Where(c => c.PrecoDeVenda <= filtro);
            }

            if (produtosParametros.Ordenacao != null)
            {
                var coluna = produtosParametros.Ordenacao;
                var direcao = produtosParametros.Direcao;

                switch (coluna)
                {
                    case 1:
                        produtosQuery = direcao == "asc" ?
                            produtosQuery.OrderBy(c => c.ProdutoCategoria.Nome) :
                            produtosQuery.OrderByDescending(c => c.ProdutoCategoria.Nome);
                        break;
                    case 4:
                        produtosQuery = direcao == "asc" ?
                            produtosQuery.OrderBy(c => c.EstoqueAtual) :
                            produtosQuery.OrderByDescending(c => c.EstoqueAtual);
                        break;
                }
            }

            var totalRegistros = await produtosQuery.CountAsync();

            var data = await produtosQuery
                         .Skip(produtosParametros.start)
                         .Take(produtosParametros.lenght)
                         .Select(c => new
                         {
                             id = c.Id,
                             categoria = c.ProdutoCategoria.Nome,
                             nome = c.Nome,
                             codigo = c.Codigo,
                             imagem = c.Imagem,
                             precocusto = c.PrecoDeCusto.ToString("C", new CultureInfo("pt-BR")),
                             precovenda = c.PrecoDeVenda.ToString("C", new CultureInfo("pt-BR")),

                             // Subconsulta para obter o estoque atual baseado nas compras e vendas
                             estoqueatual = (
                                 from produto in _minhasVendasAppContext.TransacaoDeEstoques
                                 where produto.ProdutoId == c.Id
                                 group produto by produto.ProdutoId into produtoGroup
                                 select produtoGroup.Sum(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra
                                                               ? p.Quantidade
                                                               : -p.Quantidade)
                             ).FirstOrDefault() // Caso não tenha registros, retornará 0
                         })
                         .ToListAsync();

            string json = JsonConvert.SerializeObject(new
            {
                produtosParametros.draw,
                recordsFiltered = totalRegistros,
                recordsTotal = totalRegistros,
                data
            });

            return json;
        }

        public Task Remover(int id)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarPreco(int id, decimal precoDeCusto, decimal markup, decimal precoDeVenda)
        {
            throw new NotImplementedException();
        }
    }
}
