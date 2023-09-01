using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers
{
    
    public class FornecedoresController : BaseController
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IFornecedorServico _fornecedorServico;
        private readonly IMapper _mapper;

        public FornecedoresController(IFornecedorRepositorio fornecedorRepositorio,
                                      IFornecedorServico fornecedorServico,
                                      IMapper mapper,
                                      INotificador notificador) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
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
            var fornecedor = await _fornecedorRepositorio.BuscarPorId(id);

            if (fornecedor == null) return NotFound();

            var fornecedorViewModel = _mapper.Map<FornecedorViewModel>(fornecedor);

            return View(fornecedorViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] FornecedorViewModel fornecedorViewModel)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(fornecedorViewModel);

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);

            await _fornecedorServico.Atualizar(fornecedor);

            if (!OperacaoValida()) return View(fornecedorViewModel);

            return RedirectToAction(nameof(Index));

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
    }
}
