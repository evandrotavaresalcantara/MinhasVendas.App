using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;

[Authorize]
public class DetalheDeVendasController : BaseController
{   
    private readonly IDetalheDeVendaServico _detalheDeVendaServico;
    private readonly ITransacaoDeEstoqueServico _transacaoDeEstoqueServico;
    private readonly IProdutoServico _produtoServico;
    private readonly IMapper _mapper;

    public DetalheDeVendasController(IDetalheDeVendaServico detalheDeVendaServico,
                                     ITransacaoDeEstoqueServico transacaoDeEstoqueServico,
                                     IProdutoServico produtoServico,
                                     IMapper mapper,
                                     INotificador notificador) : base(notificador)
    {        
        _detalheDeVendaServico = detalheDeVendaServico;
        _transacaoDeEstoqueServico = transacaoDeEstoqueServico;
        _produtoServico = produtoServico;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> InserirProduto(int id)
    {

        var listaDeProdutos = await _transacaoDeEstoqueServico.ConsultaSaldoDeEstoque();

        ViewData["ProdutoId"] = new SelectList(listaDeProdutos, "Id", "ProdutoCompleto");
        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        await _detalheDeVendaServico.AdicionarView(id);

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        return PartialView("_InserirProduto", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InserirProduto([Bind("Id,OrdemDeVendaId,ProdutoId,Quantidade,PrecoUnitario,Desconto")] DetalheDeVendaViewModel detalheDeVendaViewModel)  //
    {
        var listaDeProdutos = await _transacaoDeEstoqueServico.ConsultaSaldoDeEstoque();

        ViewData["ProdutoId"] = new SelectList(listaDeProdutos, "Id", "ProdutoCompleto");
        ViewData["OrdemDeVendaId"] = detalheDeVendaViewModel.OrdemDeVendaId;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();
        model.DetalheDeVendaViewModel = detalheDeVendaViewModel;

        if (!ModelState.IsValid) return PartialView("_InserirProduto", model);

        var detalheDeVenda = _mapper.Map<DetalheDeVenda>(detalheDeVendaViewModel);

        await _detalheDeVendaServico.Adicionar(detalheDeVenda);

        if (!OperacaoValida()) return PartialView("_InserirProduto", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = detalheDeVendaViewModel.OrdemDeVendaId });
               

    }

    // GET: VendasDetalhes/Delete/5
    public async Task<IActionResult> ExcluirProduto(int id)
    {
        var detalheDeVendaBD = await _detalheDeVendaServico.ConsultaDetalheDeVendaProdutoOrdemDeVenda(id);

        if (detalheDeVendaBD is null) return NotFound("Item não encontrado.");

        await _detalheDeVendaServico.Remover(id, true);

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        var detalheDeVendaViewModel = _mapper.Map<DetalheDeVendaViewModel>(detalheDeVendaBD);

        model.DetalheDeVendaViewModel = detalheDeVendaViewModel;

        return PartialView("_ExcluirProduto", model);
    }

    // POST: VendasDetalhes/Delete/5
    [HttpPost, ActionName("ExcluirProduto")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var detalheDeVendaBD = await _detalheDeVendaServico.ConsultaDetalheDeVendaOrdemDeVenda(id);

        if (detalheDeVendaBD is null) return NotFound("Item Não econtrado.");

        await _detalheDeVendaServico.Remover(id, false);

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { detalheDeVendaBD.OrdemDeVenda.Id });
    }


    [HttpGet]
    public async Task<IActionResult> ObterProdutosVenda()
    {

        var listaDeProdutos = await _transacaoDeEstoqueServico.ConsultaSaldoDeEstoque();
       
        var listaDeProdutosAutoComplete = listaDeProdutos.Select(c => new
        {
            label = c.ProdutoCompleto,
            value = c.Id,
            nome = c.NomeProduto

        });

        return Json(listaDeProdutosAutoComplete);

    }

    [HttpGet]
    public async Task<IActionResult> InserirProdutoTeste(int id)
    {

        var listaDeProdutos = await _transacaoDeEstoqueServico.ConsultaSaldoDeEstoque();

        ViewData["ProdutoId"] = new SelectList(listaDeProdutos, "Id", "ProdutoCompleto");
        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        await _detalheDeVendaServico.AdicionarView(id);

        if (!OperacaoValida()) return PartialView("_OrdemDendaStatus", model);

        return View(model);
    }


}
