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
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Repositorio;
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;

public class OrdemDeVendasController : BaseController
{       
    private readonly IOrdemDeVendaServico _ordemDeVendaServico;
    private readonly IClienteRespositorio _clienteRespositorio;
    private readonly IOrdemDeVendaRepositorio _ordemDeVendaRepositorio;

    public OrdemDeVendasController(MinhasVendasAppContext context,
                                   IOrdemDeVendaServico ordemDeVendaServico,
                                   IClienteRespositorio clienteRespositorio,
                                   IOrdemDeVendaRepositorio ordemDeVendaRepositorio,
                                   INotificador notificador) : base(notificador)
    {
        _ordemDeVendaServico = ordemDeVendaServico;
        _clienteRespositorio = clienteRespositorio;
        _ordemDeVendaRepositorio = ordemDeVendaRepositorio;
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrdemDeVenda>> Index(string ordemDeClassificacao, string filtroAtual, string pesquisarTexto, int? numeroDePagina)
    {
        var ordemDeVendasParametros = new OrdemDeVendasParametros() { NumeroDePagina = numeroDePagina ?? 1, TamanhoDePagina = 10 };

        ViewData["ClassificacaoAtual"] = ordemDeClassificacao;
        ViewData["DataDeVendaClassificarParam"] = String.IsNullOrEmpty(ordemDeClassificacao) ? "dataDeVenda_descendente" : "";
        ViewData["StatusOrdemDeVendaClassificarParam"] = ordemDeClassificacao == "statusOrdemDeVenda" ? "statusOrdemDeVenda_descendente" : "statusOrdemDeVenda";


        ordemDeVendasParametros.OrdemDeClassificacao = ordemDeClassificacao;
        ordemDeVendasParametros.PesquisaTexto = pesquisarTexto;
        ordemDeVendasParametros.FiltroAtual = filtroAtual;

        ViewData["FiltroAtual"] = ordemDeVendasParametros.PesquisaTexto ?? ordemDeVendasParametros.FiltroAtual;




        var ordemDeVendaCliente = _ordemDeVendaRepositorio.ObterOrdemDeVendasPaginacaoLista(ordemDeVendasParametros);

        var metadata = new
        {
            ordemDeVendaCliente.TotalDeItens,
            ordemDeVendaCliente.TamanhoDaPagina,
            ordemDeVendaCliente.PaginaAtual,
            ordemDeVendaCliente.TotalDePaginas,
            ordemDeVendaCliente.TemProxima,
            ordemDeVendaCliente.TemAnterior

        };

        ViewBag.Metada = metadata;

        return View(ordemDeVendaCliente);
    }

    public async Task<IActionResult> Index2()
    {
        var ordemDeVendasClientes = await _ordemDeVendaServico.ConsultaOrdemDevendaCliente();

        return View(ordemDeVendasClientes);
    }



    public async Task<IActionResult> Create()
    {
        ViewData["ClienteId"] = new SelectList(await _clienteRespositorio.BuscarTodos(), "Id", "Nome");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ClienteId,StatusOrdemDeVenda,FormaDePagamento,DataDePagamento,DataDeVenda")] OrdemDeVenda ordemDeVenda)
    {
        if (!ModelState.IsValid)
        {
            ViewData["ClienteId"] = new SelectList(await _clienteRespositorio.BuscarTodos(), "Id", "Nome", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
        }

        await _ordemDeVendaServico.Adicionar(ordemDeVenda);

        if (!OperacaoValida())
        {
            ViewData["ClienteId"] = new SelectList(await _clienteRespositorio.BuscarTodos(), "Id", "Nome", ordemDeVenda.ClienteId);
            return View(ordemDeVenda);
        }

        return RedirectToAction("CarrinhoDeVendas", new { id = ordemDeVenda.Id });
    }

    public async Task<IActionResult> CarrinhoDeVendas(int id)
    {
        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(id);

        if (ordemDeVenda == null) return NotFound("Carrinho de Compra não Existe.");

        var model = new CarrinhoDeVendasViewModel();

        model.OrdemDeVenda = ordemDeVenda;

        return View(model);
    }

    public async Task<IActionResult> CarrinhoDeVendasPartial(int id)
    {
        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalhesDeVendaClienteProduto(id);

        if (ordemDeVenda == null) return NotFound("Carrinho de Compra não EXISTE.");

        var model = new CarrinhoDeVendasViewModel();

        model.OrdemDeVenda = ordemDeVenda;

        return PartialView("CarrinhoDeVendas", model);
    }

    public async Task<IActionResult> FinalizarVenda(int id)
    {
        await _ordemDeVendaServico.FinalizarVendaView(id);

        ViewData["OrdemDeVendaId"] = id;

        CarrinhoDeVendasViewModel model = new CarrinhoDeVendasViewModel();

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVendaDetalheDeVenda(id);

        model.OrdemDeVenda = ordemDeVenda;

        return PartialView("_FinalizarVenda", model);
    }

    [HttpPost]
    public async Task<IActionResult> FinalizarVenda(int id, CarrinhoDeVendasViewModel model)
    {
        if (id != model.OrdemDeVenda.Id) return NotFound();

        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVenda(model.OrdemDeVenda.Id);

        if (ordemDeVenda is null) return NotFound();

        await _ordemDeVendaServico.FinalizarVenda(model.OrdemDeVenda);

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = ordemDeVenda.Id });

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
        if (id != model.OrdemDeVenda.Id) return NotFound();

        var ordemDeVenda = await _ordemDeVendaServico.ConsultaOrdemDeVenda(model.OrdemDeVenda.Id);

        if (ordemDeVenda == null) return NotFound("Ordem de Venda Não Econtrada.");

        await _ordemDeVendaServico.InserirFrete(model.OrdemDeVenda);

        if (!OperacaoValida()) return PartialView("_OrdemDeVendaStatus", model);

        return RedirectToAction("CarrinhoDeVendas", "OrdemDeVendas", new { id = ordemDeVenda.Id });

    }
}
