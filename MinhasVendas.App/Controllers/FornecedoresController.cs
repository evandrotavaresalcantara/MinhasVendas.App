using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers;


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

    public async Task<IActionResult> Index()
    {
        var fornecedores = await _fornecedorRepositorio.BuscarTodos();

        var fornecedoresViewModel = _mapper.Map<IEnumerable<FornecedorViewModel>>(fornecedores);

        return View(fornecedoresViewModel);

    }

    public async Task<IActionResult> Details(int id)
    {
        var fornecedorEnderecoProduto = await _fornecedorRepositorio.ObterFornecedorProdutoEndereco(id);

        if (fornecedorEnderecoProduto == null) return NotFound();

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedorEnderecoProduto);

        return View(fornecedorViewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return View(fornecedorViewModel);

        var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

        await _fornecedorServico.Adicionar(fornecedor);

        if (!OperacaoValida()) return View(fornecedorViewModel);

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Edit(int id)
    {
        var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

        if (fornecedor == null) return NotFound();

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedor);

        return View(fornecedorViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, FornecedorViewModel fornecedorViewModel)
    {
        if (!ModelState.IsValid) return View(fornecedorViewModel);

        if (id != fornecedorViewModel.Id) return NotFound();

        var fornecedorDB = await _fornecedorRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(f => f.Id == id);

        if (fornecedorDB == null) return NotFound();

        var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

        await _fornecedorServico.Atualizar(fornecedor);

        if (!OperacaoValida()) return View(fornecedorViewModel);

        return RedirectToAction("Details", new {id = fornecedor.Id});

    }

    public async Task<IActionResult> Delete(int id)
    {
        var fornecedor = await _fornecedorRepositorio.ObterPorId(m => m.Id == id);

        if (fornecedor == null) return NotFound();

        var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedor);

        return View(fornecedorViewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
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

        return RedirectToAction("Details", new { id = fornecedorEndereco.FornecedorId });

    }
}
