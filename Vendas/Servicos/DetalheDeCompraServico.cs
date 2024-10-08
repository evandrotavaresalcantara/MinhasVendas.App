﻿using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Models.Enums;
using Vendas.ViewModels;

namespace Vendas.Servicos;

public class DetalheDeCompraServico : BaseServico, IDetalheDeCompraServico
{
    private readonly VendasAppContext _minhasVendasAppContext;
    public DetalheDeCompraServico(VendasAppContext minhasVendasAppContext,
                                  INotificador notificador) : base(notificador)
    {
        _minhasVendasAppContext = minhasVendasAppContext;
    }

    public async Task Adicionar(DetalheDeCompra detalheDeCompra)
    {
        var ordemDeCompra = await _minhasVendasAppContext.OrdemDeCompras.FindAsync(detalheDeCompra.OrdemDeCompraId);

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Aprovado)
        {
            Notificar("Ordem de Compra está Aprovada.");
            return;
        }
        _minhasVendasAppContext.Add(detalheDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

    }

    public Task Atualizar(DetalheDeCompra detalheDeCompra)
    {
        throw new NotImplementedException();
    }

    public async Task RemoverStatus(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                              .Include(v => v.OrdemDeCompra)
                              .FirstOrDefaultAsync(m => m.Id == id);


        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }
        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Aprovado)
        {
            Notificar("Ordem de Compra está aprovada.");
            return;
        }
     
        if (detalheDeCompra.RegistradoTransacaoDeEstoque)
        {
            Notificar("Produto já registrado no estoque.");
            return;
        }
    }

    public async Task Remover(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                               .Include(v => v.OrdemDeCompra)
                               .FirstOrDefaultAsync(m => m.Id == id);


        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Venda está fechada. Não é possível Excluir o produto.");
            return;
        }
        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Aprovado)
        {
            Notificar("Ordem de Venda está aprovada. Não é possível Excluir o produto.");
            return;
        }

        if (detalheDeCompra.RegistradoTransacaoDeEstoque)
        {
            Notificar("Produto já registrado no estoque. Não é possível excluir.");
            return;
        }
        _minhasVendasAppContext.DetalheDeCompras.Remove(detalheDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();

    }



    public async Task InserirProdutoStatus(int id)
    {
        var ordemDeCompra = await _minhasVendasAppContext.OrdemDeCompras.FindAsync(id);

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Aprovado)
        {
            Notificar("Ordem de Compra está Aprovada.");
            return;
        }
    }

    public async Task ReceberProduto(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                              .Include(v => v.OrdemDeCompra)
                              .FirstOrDefaultAsync(m => m.Id == id);


        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }
        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Novo)
        {
            Notificar("Ordem de Compra sem solicitação de Aprovação.");
            return;
        }
        if (detalheDeCompra.OrdemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Solicitado)
        {
            Notificar("Ordem de Compra em Análise.");
            return;
        }

        if (detalheDeCompra.RegistradoTransacaoDeEstoque)
        {
            Notificar("Produto já registrado no estoque.");
            return;
        }
    }

    public async Task ReceberProduto(DetalheDeCompra detalheDeCompra)
    {
        var ordemDeCompra = await _minhasVendasAppContext.OrdemDeCompras.FindAsync(detalheDeCompra.OrdemDeCompraId);

        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Fechado)
        {
            Notificar("Ordem de Compra está fechada.");
            return;
        }
        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Novo)
        {
            Notificar("Ordem de Compra sem solicitação de Aprovação.");
            return;
        }
        if (ordemDeCompra.StatusOrdemDeCompra == StatusOrdemDeCompra.Solicitado)
        {
            Notificar("Ordem de Compra em Análise.");
            return;
        }

        if (detalheDeCompra.RegistradoTransacaoDeEstoque)
        {
            Notificar("Produto já registrado no estoque.");
            return;
        }

        var itemDetalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras.FindAsync(detalheDeCompra.Id);

        itemDetalheDeCompra.DataDeRecebimento = detalheDeCompra.DataDeRecebimento?.ToUniversalTime();
        itemDetalheDeCompra.RegistradoTransacaoDeEstoque = true;
        _minhasVendasAppContext.Update(itemDetalheDeCompra);
        await _minhasVendasAppContext.SaveChangesAsync();   

        TransacaoDeEstoque transacaoDeEstoque = new TransacaoDeEstoque();
        transacaoDeEstoque.ProdutoId = detalheDeCompra.ProdutoId;
        transacaoDeEstoque.OrdemDeCompraId = detalheDeCompra.OrdemDeCompraId;
        transacaoDeEstoque.TipoDransacaoDeEstoque = TipoDransacaoDeEstoque.Compra;
        transacaoDeEstoque.DataDeTransacao = DateTime.Now.ToUniversalTime();
        transacaoDeEstoque.Quantidade = detalheDeCompra.Quantidade;
        _minhasVendasAppContext.Add(transacaoDeEstoque);
        await _minhasVendasAppContext.SaveChangesAsync();
    }


    public async Task<DetalheDeCompra> ConsultaDetalheDeCompraProdutoOrdemDeCompra(int id)
    {
        var consultaDetalheDeCompraProdutoOrdemDeCompra = await _minhasVendasAppContext.DetalheDeCompras
                                                                 .AsNoTracking()
                                                                 .Include(p => p.Produto)
                                                                 .Include(o => o.OrdemDeCompra)
                                                                 .FirstOrDefaultAsync(d => d.Id == id);

        return consultaDetalheDeCompraProdutoOrdemDeCompra;
    }

    public async Task<DetalheDeCompra> Consulta(int id)
    {
        var detalheDeCompra = await _minhasVendasAppContext.DetalheDeCompras.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

        return detalheDeCompra;
    }

}

