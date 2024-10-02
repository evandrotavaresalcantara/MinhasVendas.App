using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Models.Enums;

namespace Vendas.Servicos;

public class DetalheDeVendaServico : BaseServico, IDetalheDeVendaServico
{

    private readonly VendasAppContext _minhasVendasAppContext;
    private readonly IDetalheDeVendaRepositorio _detalheDeVendaRepositorio;
    private readonly IProdutoRepositorio _produtoRepositorio;

    public DetalheDeVendaServico(VendasAppContext minhasVendasAppContext,
                                 IDetalheDeVendaRepositorio detalheDeVendaRepositorio,
                                 IProdutoRepositorio produtoRepositorio,
                                 INotificador notificador) : base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;
        _detalheDeVendaRepositorio = detalheDeVendaRepositorio;
        _produtoRepositorio = produtoRepositorio;

    }
    public async Task Adicionar(DetalheDeVenda detalheDeVenda)
    {
        var precoUnitario = await _produtoRepositorio.ObterSemRastreamento()
                                   .FirstOrDefaultAsync(p => p.Id == detalheDeVenda.ProdutoId);

        detalheDeVenda.PrecoUnitario = (precoUnitario.PrecoDeCusto * (precoUnitario.MarkUp / 100)) + precoUnitario.PrecoDeCusto;

         await _detalheDeVendaRepositorio.Adicionar(detalheDeVenda);
       
    }

    public async Task AdicionarView(int id)
    {
        var ordemDevenda = _minhasVendasAppContext.OrdemDeVendas.FirstOrDefault(o => o.Id == id);

        if (ordemDevenda == null)
        {
            Notificar("ADICIONAR ITEM DE VENDA - Não exsitem ordem de venda com o id informada");
            return;
        }

        if (ordemDevenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("ADICIONAR ITEM DE VENDA - Ordem de venda com staus vendido");
        }


    }

    public Task Atualizar(DetalheDeVenda detalheDeVenda)
    {
        throw new NotImplementedException();
    }

    public async Task<DetalheDeVenda> ConsultaDetalheDeVendaOrdemDeVenda(int id)
    {
        var consultaDetalheDeVendaOrdemDeVenda = await _minhasVendasAppContext.DetalheDeVendas
                                                       .AsNoTracking()
                                                       .Include(o => o.OrdemDeVenda)
                                                       .FirstOrDefaultAsync(d => d.Id == id);
        return consultaDetalheDeVendaOrdemDeVenda;
    }

    public async Task<DetalheDeVenda> ConsultaDetalheDeVendaProdutoOrdemDeVenda(int id)
    {
        var consultaDetalheDeVendaProdutoOrdemDeVenda = await _minhasVendasAppContext
                                                              .DetalheDeVendas
                                                              .AsNoTracking()
                                                              .Include(p => p.Produto)
                                                              .Include(o => o.OrdemDeVenda)
                                                              .FirstOrDefaultAsync(d => d.Id == id);

        return consultaDetalheDeVendaProdutoOrdemDeVenda;

    }

    public async Task Remover(int id, bool? ehView)
    {
        var detalheDeVenda = await _minhasVendasAppContext.DetalheDeVendas.Include(o => o.OrdemDeVenda).FirstOrDefaultAsync(o => o.Id == id);

        if (detalheDeVenda.OrdemDeVenda.Id == null)
        {
            Notificar("REMOVER ITEM DE VENDA - Não exsitem ordem de venda com o id informada");
            return;
        }

        if (detalheDeVenda.OrdemDeVenda.StatusOrdemDeVenda == StatusOrdemDeVenda.Vendido)
        {
            Notificar("REMOVER ITEM DE VENDA - Ordem de venda com staus vendido");
        }

        if (ehView == true) return;

        _minhasVendasAppContext.Remove(detalheDeVenda);
        await _minhasVendasAppContext.SaveChangesAsync();

    }
}
