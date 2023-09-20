using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Controllers
{
    [Authorize]
    public class ProdutoCategoriasController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public ProdutoCategoriasController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: ProdutoCategorias
        public async Task<IActionResult> Index()
        {
              return _context.ProdutoCategorias != null ? 
                          View(await _context.ProdutoCategorias.ToListAsync()) :
                          Problem("Entity set 'MinhasVendasAppContext.ProdutoCategorias'  is null.");
        }

        // GET: ProdutoCategorias/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ProdutoCategorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProdutoCategorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Id")] ProdutoCategoria produtoCategoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produtoCategoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produtoCategoria);
        }

        // GET: ProdutoCategorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProdutoCategorias == null)
            {
                return NotFound();
            }

            var produtoCategoria = await _context.ProdutoCategorias.FindAsync(id);
            if (produtoCategoria == null)
            {
                return NotFound();
            }
            return View(produtoCategoria);
        }

        // POST: ProdutoCategorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Nome,Descricao,Id")] ProdutoCategoria produtoCategoria)
        {
            if (id != produtoCategoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produtoCategoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProdutoCategoriaExists(produtoCategoria.Id))
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
            return View(produtoCategoria);
        }

        // GET: ProdutoCategorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: ProdutoCategorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
