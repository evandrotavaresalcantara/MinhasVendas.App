using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Repositorio;
using Newtonsoft.Json;
using System.Globalization;

namespace MinhasVendas.App.Servicos
{
    public class ProdutoServico : BaseServico, IProdutoServico
    {
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IPrecoDeProdutoHistoricoRepositorio _precoDeProdutoHistoricoRepositorio;
        private readonly ITransacaoDeEstoqueServico _transacaoDeEstoqueServico;

        public ProdutoServico(MinhasVendasAppContext minhasVendasAppContext,
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

            produto.DataDeCadastro = DateTime.Now;
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


            //if (_minhasVendasAppContext.Produtos.AsNoTracking().ToListAsync().Result.Any(p => p.Nome == produto.Nome && p.Id != produto.Id))

            if (_produtoRepositorio.Obter().AsNoTracking().ToListAsync().Result.Any(p => p.Nome == produto.Nome && p.Id != produto.Id))    
            {
                Notificar("Já existe um produto com este nome informado.");
                return;
            }

            produtoDB.Nome = produto.Nome;
            produtoDB.Codigo = produto.Codigo;
            produtoDB.ProdutoCategoriaId = produto.ProdutoCategoriaId;
            produtoDB.Descricao = produto.Descricao;
            produtoDB.Ativo = produto.Ativo;
            produtoDB.Imagem = produto.Imagem;

            await _produtoRepositorio.Atualizar(produtoDB);

            if ((produtoDB.PrecoDeCusto != produto.PrecoDeCusto) || (produtoDB.MarkUp != produto.MarkUp) || (produtoDB.PrecoDeVenda != produto.PrecoDeVenda))
            {
                AtualizarPreco(produto.Id, produto.PrecoDeCusto, produto.MarkUp, produto.PrecoDeVenda);
            }

        }

        public async Task<IEnumerable<Produto>> ConsultaProdutos()
        {
            var produtos = await _minhasVendasAppContext.Produtos.AsNoTracking().ToListAsync();

            return produtos;
        }

        public Task Remover(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AtualizarPreco(int id, decimal precoDeCusto, decimal markup, decimal precoDeVenda)
        {

            var produto = await _produtoRepositorio.ObterPorId(p => p.Id == id);

            if (produto == null)
            {
                Notificar("Produto não encontrado");
                return;
            }

            if (precoDeCusto < 0 || markup < -101 || precoDeVenda < 0)
            {
                Notificar("Dados inconsistentes. Preços menores que zero ou markup abaixo de -101 %");
                return;
            }

            if (precoDeCusto == 0 && markup == 0 && precoDeVenda >= 0)
            {
                produto.PrecoDeCusto = precoDeCusto;
                produto.MarkUp = markup;
                produto.PrecoDeVenda = precoDeVenda;


                await _produtoRepositorio.Atualizar(produto);

                var historicoPreco = new PrecoDeProdutoHistorico
                {
                    PrecoDeCusto = precoDeCusto,
                    MarkUp = markup,
                    PrecoDeVenda = precoDeVenda,
                    DataAtualizacao = DateTime.Now,
                    ProdutoId = id,
                    EstoqueAtual = await _transacaoDeEstoqueServico.EstoqueAtualPorProduto(id)
                };

                await _precoDeProdutoHistoricoRepositorio.Adicionar(historicoPreco);
                
                return;

            }

            var novoPrecoDeVenda = precoDeCusto + (precoDeCusto * markup / 100);

            if (novoPrecoDeVenda == precoDeVenda)
            {
                produto.PrecoDeCusto = precoDeCusto;
                produto.MarkUp = markup;
                produto.PrecoDeVenda = precoDeVenda;

                await _produtoRepositorio.Atualizar(produto);


                var historicoPreco = new PrecoDeProdutoHistorico
                {
                    PrecoDeCusto = precoDeCusto,
                    MarkUp = markup,
                    PrecoDeVenda = precoDeVenda,
                    DataAtualizacao = DateTime.Now,
                    ProdutoId = id,
                    EstoqueAtual = await _transacaoDeEstoqueServico.EstoqueAtualPorProduto(id)
                };

                await _precoDeProdutoHistoricoRepositorio.Adicionar(historicoPreco);

            }
            else
            {
                Notificar("Preço não atualizado. Preço de Venda inconsistente");
            }


        }

        public async Task<string> ObterProdutos(ProdutosParametros produtosParametros)
        {
            IQueryable<Produto> ordemVendasQuery = _produtoRepositorio.Obter().Include(c => c.ProdutoCategoria);

            if (!string.IsNullOrWhiteSpace(produtosParametros.search))
                {
                produtosParametros.search = produtosParametros.search.ToLower();

                    ordemVendasQuery = ordemVendasQuery.Where(c =>
                        c.Nome.ToLower().Contains(produtosParametros.search) ||
                        c.Codigo.ToLower().Contains(produtosParametros.search)
                    );
                }

            var filtro = produtosParametros.Filtro;


          

            if (filtro == 50 || filtro == 100 || filtro == 150)
            {
               

               ordemVendasQuery = ordemVendasQuery.Where(c => c.PrecoDeVenda <= filtro);
            }
            else if (filtro == 1)
            {
                try
                {
                    throw new Exception("Exceção intencional, Tradada");

                }
                catch (Exception ex)
                {

                    return ex.Message;
                }

            }else if (filtro == 2)
            {
                    throw new Exception("Exceção intencional, não tratada");


            }


            if (produtosParametros.Ordenacao != null)
            {
                var coluna = produtosParametros.Ordenacao;
                var direcao = produtosParametros.Direcao;

                switch (coluna)
                {
                    case 1:
                        ordemVendasQuery = direcao == "asc" ?
                            ordemVendasQuery.OrderBy(c => c.ProdutoCategoria.Nome) :
                            ordemVendasQuery.OrderByDescending(c => c.ProdutoCategoria.Nome);
                        break;
                    case 4:
                        ordemVendasQuery = direcao == "asc" ?
                            ordemVendasQuery.OrderBy(c => c.EstoqueAtual) :
                            ordemVendasQuery.OrderByDescending(c => c.EstoqueAtual);
                        break;
                }
            }

            var totalRegistros = await ordemVendasQuery.CountAsync();

            try
            {
                var data = await ordemVendasQuery
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
                        estoqueatual = c.EstoqueAtual,

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
            catch (Exception ex)
            {

                return $"Error: {ex.Message}";
            }
               
       
            }
        
    }
}
