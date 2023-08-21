using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;
using Newtonsoft.Json;

namespace MinhasVendas.App.Controllers;

public class OrdemDeComprasController : BaseController
{
    private readonly IOrdemDeCompraServico _ordemDeCompraServico;
    private readonly IFornecedorRepositorio _fornecedorRepositorio;
    private readonly IOrdemDeCompraRepositorio _ordemDeCompraRepositorio;


    public OrdemDeComprasController(IOrdemDeCompraServico ordemDeCompraServico,
                                    IFornecedorRepositorio fornecedorRepositorio,
                                    IOrdemDeCompraRepositorio ordemDeCompraRepositorio,
                                    INotificador notificador) : base(notificador)
    {
        _ordemDeCompraServico = ordemDeCompraServico;
        _fornecedorRepositorio = fornecedorRepositorio;
        _ordemDeCompraRepositorio = ordemDeCompraRepositorio;
    }


    [HttpGet]
    public ActionResult<IEnumerable<OrdemDeCompra>> Index(int? numeroDePagina)
    {
        var ordemDeComprasParametros = new OrdemDeComprasParametros() { NumeroDePagina = numeroDePagina ?? 1, TamanhoDePagina = 10 };

        var ordemDeCompraFornecedor = _ordemDeCompraRepositorio.ObterOrdemDecomprasPaginacaoLista(ordemDeComprasParametros);

        var metadata = new
        {
            ordemDeCompraFornecedor.TotalDeItens,
            ordemDeCompraFornecedor.TamanhoDaPagina,
            ordemDeCompraFornecedor.PaginaAtual,
            ordemDeCompraFornecedor.TotalDePaginas,
            ordemDeCompraFornecedor.TemProxima,
            ordemDeCompraFornecedor.TemAnterior

        };

        ViewBag.Metada = metadata;

        return View(ordemDeCompraFornecedor);
    }


    public async Task<IActionResult> Create()
    {
        ViewData["FornecedorId"] = new SelectList(await _fornecedorRepositorio.BuscarTodos(), "Id", "Nome");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FornecedorId,DataDeCriacao,StatusOrdemDeCompra")] OrdemDeCompra ordemDeCompra)
    {
        ViewData["FornecedorId"] = new SelectList(await _fornecedorRepositorio.BuscarTodos(), "Id", "Nome", ordemDeCompra.FornecedorId);

        if (!ModelState.IsValid) return View(ordemDeCompra);

        await _ordemDeCompraServico.Adicionar(ordemDeCompra);

        if (!OperacaoValida()) return View(ordemDeCompra);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

    }
    public async Task<IActionResult> CarrinhoDeCompras(int id)
    {
        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(id);

        if (ordemDeCompra is null) NotFound("Ordem de Compra não Existe.");

        var model = new CarrinhoDeComprasViewModel();

        model.OrdemDeCompra = ordemDeCompra;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SolicitarAprovacao(int id)
    {

        ViewData["OrdemDeCompraId"] = id;

        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        await _ordemDeCompraServico.SolicitarAprovacao(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return PartialView("_SolicitarAprovacao", model);

    }

    [HttpPost]
    public async Task<IActionResult> SolicitarAprovacao(int id, CarrinhoDeComprasViewModel model)
    {
        if (id != model.OrdemDeCompra.Id) return NotFound();

        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompra(model.OrdemDeCompra.Id);

        if (ordemDeCompra == null) return NotFound();

        await _ordemDeCompraServico.SolicitarAprovacao(model.OrdemDeCompra);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

    }


    [HttpGet]
    public async Task<IActionResult> FinalizarCompra(int id)
    {
        ViewData["OrdemDeCompraId"] = id;

        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        await _ordemDeCompraServico.FinalizarCompraView(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompraDetalheDeCompra(id);

        model.OrdemDeCompra = ordemDeCompra;

        return PartialView("_FinalizarCompra", model);
    }


    [HttpPost]
    public async Task<IActionResult> FinalizarCompra(int id, CarrinhoDeComprasViewModel model)
    {
        if (id != model.OrdemDeCompra.Id) return NotFound();

        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompra(model.OrdemDeCompra.Id);

        if (ordemDeCompra == null) return NotFound();

        await _ordemDeCompraServico.FinalizarCompra(model.OrdemDeCompra);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

    }




}

