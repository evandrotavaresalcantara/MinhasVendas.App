using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;
using Newtonsoft.Json;

namespace MinhasVendas.App.Controllers;

[Authorize]
public class OrdemDeComprasController : BaseController
{
    private readonly IOrdemDeCompraServico _ordemDeCompraServico;
    private readonly IFornecedorRepositorio _fornecedorRepositorio;
    private readonly IOrdemDeCompraRepositorio _ordemDeCompraRepositorio;
    private readonly IMapper _mapper;


    public OrdemDeComprasController(IOrdemDeCompraServico ordemDeCompraServico,
                                    IFornecedorRepositorio fornecedorRepositorio,
                                    IOrdemDeCompraRepositorio ordemDeCompraRepositorio,
                                    IMapper mapper,
                                    INotificador notificador) : base(notificador)
    {
        _ordemDeCompraServico = ordemDeCompraServico;
        _fornecedorRepositorio = fornecedorRepositorio;
        _ordemDeCompraRepositorio = ordemDeCompraRepositorio;
        _mapper = mapper;
    }


    [HttpGet]
    public ActionResult<IEnumerable<OrdemDeCompraViewModel>> Index(string ordemDeClassificacao, string filtroAtual, string pesquisarTexto, int? numeroDePagina)
    {
        var ordemDeComprasParametros = new OrdemDeComprasParametros() { NumeroDePagina = numeroDePagina ?? 1, TamanhoDePagina = 10 };
        
        ViewData["ClassificacaoAtual"] = ordemDeClassificacao;
        ViewData["DataDeCriacaoClassificarParam"] = String.IsNullOrEmpty(ordemDeClassificacao) ? "dataDeCriacao_descendente" : "";
        ViewData["StatusOrdemDeCompraClassificarParam"] = ordemDeClassificacao == "statusOrdemDeCompra" ? "statusOrdemDeCompra_descendente" : "statusOrdemDeCompra";


        ordemDeComprasParametros.OrdemDeClassificacao = ordemDeClassificacao;
        ordemDeComprasParametros.PesquisaTexto = pesquisarTexto;
        ordemDeComprasParametros.FiltroAtual = filtroAtual;

        ViewData["FiltroAtual"] = ordemDeComprasParametros.PesquisaTexto ?? ordemDeComprasParametros.FiltroAtual;

        


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

        var ordemDeCompraViewModel = _mapper.Map<IEnumerable<OrdemDeCompraViewModel>>(ordemDeCompraFornecedor);

        return View(ordemDeCompraViewModel);
    }


    public async Task<IActionResult> Create()
    {
        ViewData["FornecedorId"] = new SelectList(await _fornecedorRepositorio.BuscarTodos(), "Id", "Nome");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FornecedorId,DataDeCriacao,StatusOrdemDeCompra")] OrdemDeCompraViewModel ordemDeCompraViewModel)
    {
        ViewData["FornecedorId"] = new SelectList(await _fornecedorRepositorio.BuscarTodos(), "Id", "Nome", ordemDeCompraViewModel.FornecedorId);

        if (!ModelState.IsValid) return View(ordemDeCompraViewModel);

        var ordemDeCompra = _mapper.Map<OrdemDeCompra>(ordemDeCompraViewModel);

        await _ordemDeCompraServico.Adicionar(ordemDeCompra);

        if (!OperacaoValida()) return View(ordemDeCompraViewModel);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompra.Id });

    }
    public async Task<IActionResult> CarrinhoDeCompras(int id)
    {
        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompraDetalheDeCompraProdutoFornecedor(id);

        if (ordemDeCompra is null) NotFound("Ordem de Compra não Existe.");

        var model = new CarrinhoDeComprasViewModel();

        var ordemDeCompraViewModel = _mapper.Map<OrdemDeCompraViewModel>(ordemDeCompra);

        model.OrdemDeCompraViewModel = ordemDeCompraViewModel;

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
        if (id != model.OrdemDeCompraViewModel.Id) return NotFound();

        var ordemDeCompra = await _ordemDeCompraServico.ConsultaOrdemDeCompra(model.OrdemDeCompraViewModel.Id);

        if (ordemDeCompra == null) return NotFound();

        var ordemDeCompraViewModel = _mapper.Map<OrdemDeCompra>(model.OrdemDeCompraViewModel);

        await _ordemDeCompraServico.SolicitarAprovacao(ordemDeCompraViewModel);

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

        var ordemDeCompraViewModel = _mapper.Map<OrdemDeCompraViewModel>(ordemDeCompra);

        model.OrdemDeCompraViewModel = ordemDeCompraViewModel;

        return PartialView("_FinalizarCompra", model);
    }


    [HttpPost]
    public async Task<IActionResult> FinalizarCompra(int id, CarrinhoDeComprasViewModel model)
    {
        if (id != model.OrdemDeCompraViewModel.Id) return NotFound();

        var ordemDeCompraBusca = await _ordemDeCompraServico.ConsultaOrdemDeCompra(model.OrdemDeCompraViewModel.Id);

        if (ordemDeCompraBusca == null) return NotFound();

        var ordemDeCompra = _mapper.Map<OrdemDeCompra>(model.OrdemDeCompraViewModel);

        await _ordemDeCompraServico.FinalizarCompra(ordemDeCompra);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = ordemDeCompraBusca.Id });

    }




}

