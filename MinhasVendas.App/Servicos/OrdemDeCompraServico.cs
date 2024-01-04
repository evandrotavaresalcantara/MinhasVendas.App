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

namespace MinhasVendas.App.Servicos;

public class OrdemDeCompraServico : BaseServico, IOrdemDeCompraServico
{
    private readonly IOrdemDeCompraRepositorio _ordemDeCompraRepositorio;

    public OrdemDeCompraServico(
                                INotificador notificador,
                                IOrdemDeCompraRepositorio ordemDeCompraRepositorio) : base(notificador)
    {
        _ordemDeCompraRepositorio = ordemDeCompraRepositorio;
    }

    public async Task Adicionar(OrdemDeCompra ordemDeCompra)
    {
        ordemDeCompra.DataDeCriacao = DateTime.Now;
        ordemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Novo;

        await _ordemDeCompraRepositorio.Adicionar(ordemDeCompra);
    }

    public Task Atualizar(OrdemDeCompra ordemDeCompra)
    {
        throw new NotImplementedException();
    }

    public Task Remover(int id)
    {
        throw new NotImplementedException();
    }


    public async Task FinalizarCompra(OrdemDeCompra ordemDeCompra)
    {
        var itemOrdemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d=> d.DetalheDeCompras).FirstOrDefaultAsync(o=> o.Id == ordemDeCompra.Id);

        var temItensDeCompra = itemOrdemDeCompra.DetalheDeCompras.Any();

        var temRegistroDeEstoqueAberto = itemOrdemDeCompra.DetalheDeCompras.Any(d => d.RegistradoTransacaoDeEstoque == false);

        if (!temItensDeCompra)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra vazia");
            return;
        }

        if (temRegistroDeEstoqueAberto)
        {
            Notificar("FINALIZAR COMPRA. Existe produto sem recebimento.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra já está fechada.");
            return;
        }

        itemOrdemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Fechado;
        itemOrdemDeCompra.FormaDePagamento = ordemDeCompra.FormaDePagamento;

        await _ordemDeCompraRepositorio.Atualizar(itemOrdemDeCompra);

    }



    public async Task FinalizarCompraView(int id)
    {
          var ordemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d => d.DetalheDeCompras).FirstOrDefaultAsync(o => o.Id == id);


        var temItensDeCompra = ordemDeCompra.DetalheDeCompras.Any();

        var temRegistroDeEstoqueAberto = ordemDeCompra.DetalheDeCompras.Any(d => d.RegistradoTransacaoDeEstoque == false);

        if (!temItensDeCompra)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra vazia");
            return;
        }

        if (temRegistroDeEstoqueAberto)
        {
            Notificar("FINALIZAR COMPRA. Existe produto sem recebimento.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("FINALIZAR COMPRA. Ordem de Compra já está fechada.");
            return;
        }

    }

    public async Task SolicitarAprovacao(OrdemDeCompra ordemDeCompra)
    {
        var itemOrdemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d => d.DetalheDeCompras).FirstOrDefaultAsync(o => o.Id == ordemDeCompra.Id);

        var temItensDeCompra = itemOrdemDeCompra.DetalheDeCompras.Any();

        var temRegistroDeEstoqueAberto = itemOrdemDeCompra.DetalheDeCompras.Any(d => d.RegistradoTransacaoDeEstoque == true);

        if (!temItensDeCompra)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de Compra vazia");
            return;
        }

        if (temRegistroDeEstoqueAberto)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de compra já está aprovada.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de Compra já está fechada.");
            return;
        }

        itemOrdemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Aprovado;
        itemOrdemDeCompra.ValorDeFrete = ordemDeCompra.ValorDeFrete;

        await _ordemDeCompraRepositorio.Atualizar(itemOrdemDeCompra);

    }

    public async Task SolicitarAprovacao(int id)
    {
        var ordemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d => d.DetalheDeCompras).FirstOrDefaultAsync(o => o.Id == id);


        var temItensDeCompra = ordemDeCompra.DetalheDeCompras.Any();

        var temRegistroDeEstoqueAberto = ordemDeCompra.DetalheDeCompras.Any(d => d.RegistradoTransacaoDeEstoque == false);

        if (!temItensDeCompra)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de Compra vazia");
            return;
        }
       
        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Aprovado)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de compra já está aprovada.");
            return;
        }
        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Solicitado)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de compra já está em Análise (já foi solicitado).");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("SOLICITAR APROVAÇÃO. Ordem de Compra já está fechada.");
            return;
        }

        //ordemDeCompra.StatusOrdemDeCompra = StatusOrdemDeCompra.Aprovado;
        //await _ordemDeCompraRepositorio.Atualizar(ordemDeCompra);
    }

    public async Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(int id)
    {
        var ordemDeCompra = await _ordemDeCompraRepositorio.Obter()
                                    .Include(d => d.DetalheDeCompras)
                                        .ThenInclude(p=> p.Produto)
                                    .Include(f=> f.Fornecedor)
                                    .FirstOrDefaultAsync(o => o.Id == id);

        return ordemDeCompra;
    }

    public async Task<OrdemDeCompra> ConsultaOrdemDeCompraDetalheDeCompra(int id)
    {

        var ordemDeCompra = await _ordemDeCompraRepositorio.Obter().Include(d => d.DetalheDeCompras).FirstOrDefaultAsync(o => o.Id == id);
      
        return ordemDeCompra;
    }

    public async Task<OrdemDeCompra> ConsultaOrdemDeCompra(int id)
    {
        var ordemDeCompra = await _ordemDeCompraRepositorio.BuscarPorId(id);
       
        return ordemDeCompra;
    }

    public async Task<IEnumerable<OrdemDeCompra>> ConsultaOrdemDeCompraFornecedor()
    {
        var ordeDecompraFornecedor = await _ordemDeCompraRepositorio.ObterSemRastreamento().Include(f=> f.Fornecedor).ToListAsync();
     
        return ordeDecompraFornecedor;
    }

    public async Task<string> ObterOrdemCompras(OrdemDeComprasParametros ordemDeComprasParametros)
    {
        IQueryable<OrdemDeCompra> ordemComprasQuery = _ordemDeCompraRepositorio.Obter();

        if (!string.IsNullOrWhiteSpace(ordemDeComprasParametros.search))
        {
            ordemDeComprasParametros.search = ordemDeComprasParametros.search.ToLower();

            ordemComprasQuery = ordemComprasQuery.Where(c =>
                c.Fornecedor.Nome.ToLower().Contains(ordemDeComprasParametros.search) ||
                c.Fornecedor.Instagram.ToLower().Contains(ordemDeComprasParametros.search)
            );
        }

        if (Enum.TryParse(ordemDeComprasParametros.Filtro, out StatusOrdemDeCompra resultado))
        {
            ordemComprasQuery = ordemComprasQuery.Where(c => c.StatusOrdemDeCompra == resultado);
        }


        if (ordemDeComprasParametros.Ordenacao != null)
        {
            var coluna = ordemDeComprasParametros.Ordenacao;
            var direcao = ordemDeComprasParametros.Direcao;

            switch (coluna)
            {
                case 3:
                    ordemComprasQuery = direcao == "asc" ?
                        ordemComprasQuery.OrderBy(c => c.StatusOrdemDeCompra) :
                        ordemComprasQuery.OrderByDescending(c => c.StatusOrdemDeCompra);
                    break;
                case 1:
                    ordemComprasQuery = direcao == "asc" ?
                        ordemComprasQuery.OrderBy(c => c.DataDeCriacao) :
                        ordemComprasQuery.OrderByDescending(c => c.DataDeCriacao);
                    break;
            }
        }

        var totalRegistros = await ordemComprasQuery.CountAsync();

        var data = await ordemComprasQuery
            .Skip(ordemDeComprasParametros.start)
            .Take(ordemDeComprasParametros.lenght)
            .Select(c => new
            {
                id = c.Id,
                nomefornecedor = c.Fornecedor.Nome,
                datadecriacao = c.DataDeCriacao.ToString("yyyy-MM-dd"),
                statusOrdemCompra = c.StatusOrdemDeCompra.ToString(),
                valorfrete = c.ValorDeFrete.ToString("C", new CultureInfo("pt-BR")),

            })
            .ToListAsync();

        string json = JsonConvert.SerializeObject(new
        {
            ordemDeComprasParametros.draw,
            recordsFiltered = totalRegistros,
            recordsTotal = totalRegistros,
            data
        });

        return json;
    }
}
