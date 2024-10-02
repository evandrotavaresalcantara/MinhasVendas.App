using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Models.Enums;
using Vendas.Paginacao;
using Vendas.Repositorio;
using Vendas.ViewModels;
using Newtonsoft.Json;

namespace Vendas.Servicos;

public class OrdemDeVendaServico : BaseServico, IOrdemDeVendaServico
{
    private readonly VendasAppContext _minhasVendasAppContext;
    private readonly IOrdemDeVendaRepositorio _ordemDeVendaRepositorio;
    public OrdemDeVendaServico(VendasAppContext minhasVendasAppContext,
                               IOrdemDeVendaRepositorio ordemDeVendaRepositorio,
                               INotificador notificador) : base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;
        _ordemDeVendaRepositorio = ordemDeVendaRepositorio;
    }
    public async Task FinalizarVendaView(int id)
    {
        var ordemDeVenda = await
            _minhasVendasAppContext.OrdemDeVendas
                .Include(v => v.DetalheDeVendas)
                    .FirstOrDefaultAsync(v => v.Id == id);

        var temItensDeVenda = ordemDeVenda.DetalheDeVendas.Any();

        if (!temItensDeVenda)
        {
            Notificar("FINALIZAR VENDA. Ordem de Venda vazia.");
            return;
        }

        if (ordemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("FINALIZAR VENDA. Ordem de venda já está com status vendido.");
            return;
        }
    }

    public async Task FinalizarVenda(OrdemDeVenda ordemDeVendaEntrada)
    {
        var ordemDeVenda = await _minhasVendasAppContext.OrdemDeVendas.Include(d => d.DetalheDeVendas).FirstOrDefaultAsync(o => o.Id == ordemDeVendaEntrada.Id);

        var temItensDeVenda = ordemDeVenda.DetalheDeVendas.Any();

        if (!temItensDeVenda)
        {
            Notificar("FINALIZAR VENDA. Ordem de Venda vazia.");
            return;
        }

        if (ordemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("FINALIZAR VENDA. Ordem de venda já está com status vendido.");
            return;
        }

        foreach (var item in ordemDeVenda.DetalheDeVendas)
        {
            TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();

            transacaoDeEstoque.ProdutoId = item.ProdutoId;
            transacaoDeEstoque.OrdemDeVendaId = item.OrdemDeVendaId;
            transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Venda;
            transacaoDeEstoque.DataDeTransacao = DateTime.Now.ToUniversalTime();
            transacaoDeEstoque.Quantidade = item.Quantidade;

            _minhasVendasAppContext.TransacaoDeEstoques.Add(transacaoDeEstoque);
            await _minhasVendasAppContext.SaveChangesAsync();

            item.TransacaoDeEstoqueId = transacaoDeEstoque.Id;
            item.RegistroTransacaoDeEstoque = true;

            _minhasVendasAppContext.DetalheDeVendas.Update(item);
            await _minhasVendasAppContext.SaveChangesAsync();

        }
        ordemDeVenda.StatusOrdemDeVenda = StatusOrdemDeVenda.Vendido;
        ordemDeVenda.FormaDePagamento = ordemDeVendaEntrada.FormaDePagamento;
        ordemDeVenda.DataDePagamento = DateTime.Now.ToUniversalTime();

        _minhasVendasAppContext.Update(ordemDeVenda);
        await _minhasVendasAppContext.SaveChangesAsync();
    }
    public async Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(int id)
    {
        var ordemDeVenda = await _minhasVendasAppContext.OrdemDeVendas
               .Include(v => v.Cliente)
               .Include(v => v.DetalheDeVendas).ThenInclude(v => v.Produto)
               .FirstOrDefaultAsync(m => m.Id == id);

        return ordemDeVenda;
    }

    public async Task Adicionar(OrdemDeVenda ordemDeVenda)
    {
        _minhasVendasAppContext.OrdemDeVendas.Add(ordemDeVenda);
        
        ordemDeVenda.StatusOrdemDeVenda = StatusOrdemDeVenda.Orcamento;
        ordemDeVenda.DataDeCriacao = DateTime.Now.ToUniversalTime();
        
        await _minhasVendasAppContext.SaveChangesAsync();
    }

    public Task Atualizar(OrdemDeVenda ordemDeVenda)
    {
        throw new NotImplementedException();
    }

    public Task Remover(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrdemDeVenda> ConsultaOrdemDeVendaDetalheDeVenda(int id)
    {
        var ordemDeVenda = await _minhasVendasAppContext
                            .OrdemDeVendas
                                .Include(v => v.DetalheDeVendas)
                            .FirstOrDefaultAsync(v => v.Id == id);
      
        return ordemDeVenda;
    }

    public async  Task<OrdemDeVenda> ConsultaOrdemDeVenda(int id)
    {
        var ordemDeVenda = await _minhasVendasAppContext.OrdemDeVendas.FindAsync(id);

        return ordemDeVenda;
    }

    public async Task<IEnumerable<OrdemDeVenda>> ConsultaOrdemDevendaCliente()
    {
        var ordemDeVendaCliente = await _minhasVendasAppContext.OrdemDeVendas.AsNoTracking().Include(c => c.Cliente).ToListAsync();

        return ordemDeVendaCliente;
    }
    public async Task InserirFrete(int id)
    {
        var ordemDeVenda = await _ordemDeVendaRepositorio.Obter().Include(d => d.DetalheDeVendas).FirstOrDefaultAsync(o => o.Id == id);

        var temItensDeVenda = ordemDeVenda.DetalheDeVendas.Any();

        if (ordemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("INSERIR VALOR DO FRETE. Ordem de venda com status VENDIDO.)");
            return;
        }
    }

    public async Task InserirFrete(OrdemDeVenda ordemDeVenda)
    {
        var ordemDeVendaBD = await _ordemDeVendaRepositorio.Obter().Include(d => d.DetalheDeVendas).FirstOrDefaultAsync(o => o.Id == ordemDeVenda.Id);

        var temItensDeVenda = ordemDeVendaBD.DetalheDeVendas.Any();

        if (ordemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("INSERIR VALOR DO FRETE. Ordem de venda com status VENDIDO.)");
            return;
        }

        ordemDeVendaBD.ValorDeFrete = ordemDeVenda.ValorDeFrete;

        await _ordemDeVendaRepositorio.Atualizar(ordemDeVendaBD);


    }

    public async Task<string> ObterOrdemVendas(OrdemDeVendasParametros ordemDeVendasParametros)
    {
        var ordemVendasQuery = _ordemDeVendaRepositorio.Obter();

        if (!string.IsNullOrWhiteSpace(ordemDeVendasParametros.search))
        {
            ordemDeVendasParametros.search = ordemDeVendasParametros.search.ToLower();

            ordemVendasQuery = ordemVendasQuery.Where(c =>
                c.Cliente.Nome.ToLower().Contains(ordemDeVendasParametros.search) ||
                c.Cliente.SobreNome.ToLower().Contains(ordemDeVendasParametros.search)
            );
        }

        if (Enum.TryParse(ordemDeVendasParametros.Filtro, out StatusOrdemDeVenda resultado))
        {
            ordemVendasQuery = ordemVendasQuery.Where(c => c.StatusOrdemDeVenda == resultado);
        }


        if (ordemDeVendasParametros.Ordenacao != null)
        {
            var coluna = ordemDeVendasParametros.Ordenacao;
            var direcao = ordemDeVendasParametros.Direcao;

            switch (coluna)
            {
                case 3: 
                    ordemVendasQuery = direcao == "asc" ?
                        ordemVendasQuery.OrderBy(c => c.StatusOrdemDeVenda) :
                        ordemVendasQuery.OrderByDescending(c => c.StatusOrdemDeVenda);
                    break;
                case 2:
                    ordemVendasQuery = direcao == "asc" ?
                        ordemVendasQuery.OrderBy(c => c.DataDeCriacao) :
                        ordemVendasQuery.OrderByDescending(c => c.DataDeCriacao);
                    break;
            }
        }

        var totalRegistros = await ordemVendasQuery.CountAsync();

        var data = await ordemVendasQuery
            .Skip(ordemDeVendasParametros.start)
            .Take(ordemDeVendasParametros.lenght)
            .Select(c => new
            {
                id = c.Id,
                nomeclinete = c.Cliente.Nome,
                datadecriacao = c.DataDeCriacao.ToString("yyyy-MM-dd"),
                statusOrdemVenda = c.StatusOrdemDeVenda.ToString(),
                formapagamento = c.FormaDePagamento.ToString(),
                datapagamento = c.DataDePagamento.HasValue ? c.DataDePagamento.Value.ToString("yyyy-MM-dd") : null,


            })
            .ToListAsync();

        string json = JsonConvert.SerializeObject(new
        {
            ordemDeVendasParametros.draw,
            recordsFiltered = totalRegistros,
            recordsTotal = totalRegistros,
            data
        });

        return json;
    }
}
