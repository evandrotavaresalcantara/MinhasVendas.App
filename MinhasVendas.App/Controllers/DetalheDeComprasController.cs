using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Notificador;
using MinhasVendas.App.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MinhasVendas.App.Controllers;

public class DetalheDeComprasController : BaseController
{
    private readonly IDetalheDeCompraServico _detalheDeCompraServico;
    private readonly IMapper _mapper;

    public DetalheDeComprasController(IDetalheDeCompraServico detalheDeCompraServico,
                                      IMapper mapper,
                                      INotificador notificador) : base(notificador)
    {
        _detalheDeCompraServico = detalheDeCompraServico;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> InserirProduto(int id)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        await _detalheDeCompraServico.InserirProdutoStatus(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        ViewData["OrdemDeCompraId"] = id;

        return PartialView("_InserirProduto", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> InserirProduto([Bind("OrdemDeCompraId,ProdutoId,Quantidade,CustoUnitario")] DetalheDeCompraViewModel detalheDeCompraViewModel)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        ViewData["OrdemDeCompraId"] = detalheDeCompraViewModel.OrdemDeCompraId;

        if (!ModelState.IsValid) return PartialView("_InserirProduto", model);

        var detalheDeCompra = _mapper.Map<DetalheDeCompra>(detalheDeCompraViewModel);

        await _detalheDeCompraServico.Adicionar(detalheDeCompra);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompraViewModel.OrdemDeCompraId });
    }

    [HttpGet]
    public async Task<IActionResult> ReceberProduto(int id)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        var detalheDeCompraBusca = await _detalheDeCompraServico.ConsultaDetalheDeCompraProdutoOrdemDeCompra(id);

        if (detalheDeCompraBusca == null) return NotFound("Item de Venda não encontrado");

        await _detalheDeCompraServico.ReceberProduto(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        var detalheDeCompraViewModel = _mapper.Map<DetalheDeCompraViewModel>(detalheDeCompraBusca);

        model.DetalheDeCompraViewModel = detalheDeCompraViewModel;

        model.DetalheDeCompraViewModel.DataDeRecebimento = DateTime.Now;

        return PartialView("_ReceberProduto", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReceberProduto(CarrinhoDeComprasViewModel carrinhoDeComprasViewModel)
    {

        var detalheDeCompra = await _detalheDeCompraServico.Consulta(carrinhoDeComprasViewModel.DetalheDeCompraViewModel.Id);

        if (detalheDeCompra == null) return NotFound("Item não encontrado.");

        detalheDeCompra.DataDeRecebimento = carrinhoDeComprasViewModel.DetalheDeCompraViewModel.DataDeRecebimento;

        await _detalheDeCompraServico.ReceberProduto(detalheDeCompra);

        if (!OperacaoValida()) return PartialView("_ReceberProduto", carrinhoDeComprasViewModel);

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });
    }

    [HttpGet]
    public async Task<IActionResult> ExcluirProduto(int id)
    {
        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();

        var detalheDeCompra = await _detalheDeCompraServico.ConsultaDetalheDeCompraProdutoOrdemDeCompra(id);

        if (detalheDeCompra == null) return NotFound("Item não encontrado.");

        var detalheDeCompraViewModel = _mapper.Map<DetalheDeCompraViewModel>(detalheDeCompra);

        model.DetalheDeCompraViewModel = detalheDeCompraViewModel;

        await _detalheDeCompraServico.RemoverStatus(id);

        if (!OperacaoValida()) return PartialView("_OrdemDeCompraStatus", model);

        return PartialView("_ExcluirProduto", model);
    }

    [HttpPost, ActionName("ExcluirProduto")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var detalheDeCompra = await _detalheDeCompraServico.Consulta(id);

        if (detalheDeCompra is null) return NotFound("Item não encontrado");

        CarrinhoDeComprasViewModel model = new CarrinhoDeComprasViewModel();


        var detalheDeCompraViewModel = _mapper.Map<DetalheDeCompraViewModel>(detalheDeCompra);

        model.DetalheDeCompraViewModel = detalheDeCompraViewModel;

        await _detalheDeCompraServico.Remover(id);

        if (!OperacaoValida()) return PartialView("_ExcluirProduto", model); // Implementar Modal Ajax. Não chega aqui por causa do Get ExcluirProduto() "VerificarStatus"

        return RedirectToAction("CarrinhoDeCompras", "OrdemDeCompras", new { id = detalheDeCompra.OrdemDeCompraId });

    }
}
