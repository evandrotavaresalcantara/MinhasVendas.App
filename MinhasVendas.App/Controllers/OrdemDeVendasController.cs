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
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Repositorio;
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;

[Authorize]
public class OrdemDeVendasController : BaseController
{       
    private readonly IOrdemDeVendaServico _ordemDeVendaServico;
    private readonly IClienteRespositorio _clienteRespositorio;
    private readonly IOrdemDeVendaRepositorio _ordemDeVendaRepositorio;
    private readonly IMapper _mapper;

    public OrdemDeVendasController(MinhasVendasAppContext context,
                                   IOrdemDeVendaServico ordemDeVendaServico,
                                   IClienteRespositorio clienteRespositorio,
                                   IOrdemDeVendaRepositorio ordemDeVendaRepositorio,
                                   IMapper mapper,
                                   INotificador notificador) : base(notificador)
    {
        _ordemDeVendaServico = ordemDeVendaServico;
        _clienteRespositorio = clienteRespositorio;
        _ordemDeVendaRepositorio = ordemDeVendaRepositorio;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrdemDeVenda>> Index()
    {
        return View();
    }


    public async Task<IActionResult> Create()
    {
        ViewData["ClienteId"] = new SelectList(await _clienteRespositorio.BuscarTodos(), "Id", "Nome");
        return View();


    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ClienteId,StatusOrdemDeVenda,FormaDePagamento,DataDePagamento,DataDeVenda")] OrdemDeVendaViewModel ordemDeVendaViewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ClienteId"] = new SelectList(await _clienteRespositorio.BuscarTodos(), "Id", "Nome", ordemDeVendaViewModel.ClienteId);
            return View(ordemDeVendaViewModel);
        }

        var ordemDeVenda = _mapper.Map<OrdemDeVenda>(ordemDeVendaViewModel);
        
        await _ordemDeVendaServico.Adicionar(ordemDeVenda);

        if (!OperacaoValida())
        {
            ViewData["ClienteId"] = new SelectList(await _clienteRespositorio.BuscarTodos(), "Id", "Nome", ordemDeVendaViewModel.ClienteId);
            return View(ordemDeVendaViewModel);
        }

        return RedirectToAction("CarrinhoDeVendas", new { id = ordemDeVenda.Id });
    }

    public async Task<IActionResult> CarrinhoDeVendas(int id)
    {
        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(id);

        if (ordemDeVenda == null) return NotFound("Carrinho de Compra não Existe.");

        var model = new CarrinhoDeVendasViewModel();

        var ordemDeVendaViewModel = _mapper.Map<OrdemDeVendaViewModel>(ordemDeVenda);

        model.OrdemDeVendaViewModel = ordemDeVendaViewModel;

        return View(model);
    }

    public async Task<IActionResult> CarrinhoDeVendasPartial(int id)
    {
        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(id);

        if (ordemDeVenda == null) return NotFound("Carrinho de Compra não EXISTE.");

        var model = new CarrinhoDeVendasViewModel();

        var ordemDeVendaViewModel = _mapper.Map<OrdemDeVendaViewModel>(ordemDeVenda);
        
        model.OrdemDeVendaViewModel = ordemDeVendaViewModel;

        return PartialView("CarrinhoDeVendas", model);
    }

    public async Task<IActionResult> FinalizarVenda(int id)
    {
        await _ordemDeVendaServico.FinalizarVendaView(id);

        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalheDeVenda(id);

        var ordemDeVendaViewModel = _mapper.Map<OrdemDeVendaViewModel>(ordemDeVenda);

        model.OrdemDeVendaViewModel = ordemDeVendaViewModel;

        return PartialView("_FinalizarVenda", model);
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarVenda(int id, CarrinhoDeVendasViewModel model)
    {
        if (id != model.OrdemDeVendaViewModel.Id) return NotFound();

        var ordemDeVendaBD = await _ordemDeVendaServico.ConsultaOrdemDeVenda(model.OrdemDeVendaViewModel.Id);

        if (ordemDeVendaBD is null) return NotFound();

        var ordemDeVenda = _mapper.Map<OrdemDeVenda>(model.OrdemDeVendaViewModel);

        await _ordemDeVendaServico.FinalizarVenda(ordemDeVenda);

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = ordemDeVendaBD.Id });

    }

    [HttpGet]
    public async Task<IActionResult> InserirFrete(int id)
    {

        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        await _ordemDeVendaServico.InserirFrete(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        return PartialView("_InserirFrete", model);

    }

    [HttpPost]
    public async Task<IActionResult> InserirFrete(int id, CarrinhoDeVendasViewModel model)
    {
        if (id != model.OrdemDeVendaViewModel.Id) return NotFound();

        var ordemDeVendaBD = await _ordemDeVendaServico.ConsultaOrdemDeVenda(model.OrdemDeVendaViewModel.Id);

        if (ordemDeVendaBD == null) return NotFound("Ordem de Venda Não Econtrada.");

        var ordemDeVenda = _mapper.Map<OrdemDeVenda>(model.OrdemDeVendaViewModel);

        await _ordemDeVendaServico.InserirFrete(ordemDeVenda);

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = ordemDeVendaBD.Id });

    }

    public async Task<IActionResult> ObterOrdemVendas(int draw, int start, int length, string statusFiltro, 
                                                     [FromQuery(Name = "search[value]")] string search,
                                                     [FromQuery(Name = "order[0][column]")] int ordenacao,
                                                     [FromQuery(Name = "order[0][dir]")] string direcao)
                                                     
    {

        var parametros = new OrdemDeVendasParametros();

        parametros.draw = draw;
        parametros.start = start;
        parametros.lenght = length;
        parametros.search = search;
        parametros.Filtro = statusFiltro;
        parametros.Ordenacao = ordenacao;
        parametros.Direcao = direcao;



        string json = await _ordemDeVendaServico.ObterOrdemVendas(parametros);

        return Content(json, "application/json");
    }
}
