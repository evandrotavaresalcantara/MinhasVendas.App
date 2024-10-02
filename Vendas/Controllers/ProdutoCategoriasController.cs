using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Models;
using Vendas.ViewModels;

namespace Vendas.Controllers
{
    [Authorize]
    public class ProdutoCategoriasController : Controller
    {
        private readonly VendasAppContext _context;
        private readonly IMapper _mapper;

        public ProdutoCategoriasController(VendasAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<IActionResult> Index()
        {
            return _context.ProdutoCategorias != null ?
                        View(await _context.ProdutoCategorias.ToListAsync()) :
                        Problem("Entity set 'MinhasVendasAppContext.ProdutoCategorias'  is null.");
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo([Bind("Nome,Descricao,Id")] ProdutoCategoriaViewModel produtoCategoriaViewModel)
        {
            if (!ModelState.IsValid) return View(produtoCategoriaViewModel);

            var produtoCategoria = _mapper.Map<ProdutoCategoria>(produtoCategoriaViewModel);

            _context.Add(produtoCategoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Atualizar(int? id)
        {
            if (id == null || _context.ProdutoCategorias == null) return NotFound();
            
            var produtoCategoria = await _context.ProdutoCategorias.FindAsync(id);
            if (produtoCategoria == null)  return NotFound();

            var produtoCategoriaViewModel = _mapper.Map<ProdutoCategoriaViewModel>(produtoCategoria);

            return View(produtoCategoriaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(int id, [Bind("Nome,Descricao,Id")] ProdutoCategoria produtoCategoriaViewModel)
        {
            if (id != produtoCategoriaViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var produtoCategoria = _mapper.Map<ProdutoCategoria>(produtoCategoriaViewModel);
                    _context.Update(produtoCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoCategoriaExists(produtoCategoriaViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produtoCategoriaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.ProdutoCategorias == null)
            {
                return NotFound();
            }

            var produtoCategoria = await _context.ProdutoCategorias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produtoCategoria == null)
            {
                return NotFound();
            }

            return View(produtoCategoria);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            if (_context.ProdutoCategorias == null)
            {
                return Problem("Entity set 'MinhasVendasAppContext.ProdutoCategorias'  is null.");
            }
            var produtoCategoria = await _context.ProdutoCategorias.FindAsync(id);
            if (produtoCategoria != null)
            {
                _context.ProdutoCategorias.Remove(produtoCategoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProdutoCategoriaExists(int id)
        {
            return (_context.ProdutoCategorias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
