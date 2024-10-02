using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Paginacao;
using Vendas.ViewModels;

namespace Vendas.Controllers;

[Authorize]
public class FornecedoresController : BaseController
{
    private readonly IFornecedorRepositorio _fornecedorRepositorio;
    private readonly IFornecedorEnderecoRepositorio _fornecedorEnderecoRepositorio;
    private readonly IFornecedorServico _fornecedorServico;
    private readonly IMapper _mapper;

    public FornecedoresController(IFornecedorRepositorio fornecedorRepositorio,
                                  IFornecedorEnderecoRepositorio fornecedorEnderecoRepositorio,
                                  IFornecedorServico fornecedorServico,
                                  IMapper mapper,
                                  INotificador notificador) : base(notificador)
    {
        _fornecedorRepositorio = fornecedorRepositorio;
        _fornecedorEnderecoRepositorio = fornecedorEnderecoRepositorio;
        _fornecedorServico = fornecedorServico;
        _mapper = mapper;
        
    }

    [HttpGet]
    public ActionResult<IEnumerable<OrdemDeCompraViewModel>> Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> ObterFornecedores(int draw, int start, int length, string statusFiltro,
                                                    [FromQuery(Name = "search[value]")] string search,
                                                    [FromQuery(Name = "order[0][column]")] int ordenacao,
                                                    [FromQuery(Name = "order[0][dir]")] string direcao)
    {

        var parametros = new FornecedoresParametros();


        parametros.draw = draw;
        parametros.start = start;
        parametros.lenght = length;
        parametros.search = search;
        parametros.Filtro = statusFiltro;
        parametros.Ordenacao = ordenacao;
        parametros.Direcao = direcao;

        string json = await _fornecedorServico.ObterFornecedores(parametros);

        return Content(json, "application/json");
    }

    [HttpGet]
    public async Task<IActionResult> Obter(int id)
    {
        var fornecedorEnderecoProduto = await _fornecedorRepositorio.ObterFornecedorProdutoEndereco(id);

        if (fornecedorEnderecoProduto == null) return NotFound();

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedorEnderecoProduto);

        return View(fornecedorViewModel);
    }

    [HttpGet]
    public IActionResult Novo()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Novo(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return View(fornecedorViewModel);

        var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

        await _fornecedorServico.Adicionar(fornecedor);

        if (!OperacaoValida()) return View(fornecedorViewModel);

        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> Atualizar(int id)
    {
        var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

        if (fornecedor == null) return NotFound();

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedor);

        return View(fornecedorViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Atualizar(int id, FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return View(fornecedorViewModel);

        if (id != fornecedorViewModel.Id) return NotFound();

        var fornecedorDB = await _fornecedorRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(f => f.Id == id);

        if (fornecedorDB == null) return NotFound();

        var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

        await _fornecedorServico.Atualizar(fornecedor);

        if (!OperacaoValida()) return View(fornecedorViewModel);

        return RedirectToAction("Obter", new {id = fornecedor.Id});

    }

    [HttpGet]
    public async Task<IActionResult> Excluir(int id)
    {
        var fornecedor = await _fornecedorRepositorio.ObterPorId(m => m.Id == id);

        if (fornecedor == null) return NotFound();

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedor);

        return View(fornecedorViewModel);
    }

    [HttpPost, ActionName("Excluir")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

        if (fornecedor == null) return NotFound();

        _fornecedorRepositorio.Desanexar(fornecedor);

        await _fornecedorServico.Remover(id);

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedor);

        if (!OperacaoValida()) return View(fornecedorViewModel);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditarEnderecoFornecedor(int id)
    {
        var endereco = await _fornecedorEnderecoRepositorio.ObterPorId(e => e.Id == id);

        if (endereco == null) return NotFound();

        var fornecedorEnderecoViewModel = _mapper.Map<FornecedorEnderecoViewModel>(endereco);

        return PartialView("_AtualizarEnderecoFornecedor", fornecedorEnderecoViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditarEnderecoFornecedor(FornecedorEnderecoViewModel fornecedorEnderecoViewModel)
    {
        if (!ModelState.IsValid) return PartialView("_AtualizarEnderecoFornecedor", fornecedorEnderecoViewModel);

        var endereco = await _fornecedorEnderecoRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(e => e.Id == fornecedorEnderecoViewModel.Id);

        if (endereco == null) return NotFound();

        var fornecedorEndereco = _mapper.Map<FornecedorEndereco>(fornecedorEnderecoViewModel);

        await _fornecedorEnderecoRepositorio.Atualizar(fornecedorEndereco);

        if (!OperacaoValida()) return View(fornecedorEnderecoViewModel);

        return RedirectToAction("Obter", new { id = fornecedorEndereco.FornecedorId });

    }
   
}
