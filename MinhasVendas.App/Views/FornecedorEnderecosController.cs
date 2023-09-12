using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Views
{
    public class FornecedorEnderecosController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public FornecedorEnderecosController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: FornecedorEnderecos
        public async Task<IActionResult> Index()
        {
            var minhasVendasAppContext = _context.FornecedorEnderecoViewModel.Include(f => f.Fornecedor);
            return View(await minhasVendasAppContext.ToListAsync());
        }

        // GET: FornecedorEnderecos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FornecedorEnderecoViewModel == null)
            {
                return NotFound();
            }

            var fornecedorEnderecoViewModel = await _context.FornecedorEnderecoViewModel
                .Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedorEnderecoViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorEnderecoViewModel);
        }

        // GET: FornecedorEnderecos/Create
        public IActionResult Create()
        {
            ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Id");
            return View();
        }

        // POST: FornecedorEnderecos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FornecedorId,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Estado")] FornecedorEnderecoViewModel fornecedorEnderecoViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fornecedorEnderecoViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Id", fornecedorEnderecoViewModel.FornecedorId);
            return View(fornecedorEnderecoViewModel);
        }

        // GET: FornecedorEnderecos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FornecedorEnderecoViewModel == null)
            {
                return NotFound();
            }

            var fornecedorEnderecoViewModel = await _context.FornecedorEnderecoViewModel.FindAsync(id);
            if (fornecedorEnderecoViewModel == null)
            {
                return NotFound();
            }
            ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Id", fornecedorEnderecoViewModel.FornecedorId);
            return View(fornecedorEnderecoViewModel);
        }

        // POST: FornecedorEnderecos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FornecedorId,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Estado")] FornecedorEnderecoViewModel fornecedorEnderecoViewModel)
        {
            if (id != fornecedorEnderecoViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fornecedorEnderecoViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorEnderecoViewModelExists(fornecedorEnderecoViewModel.Id))
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
            ViewData["FornecedorId"] = new SelectList(_context.Set<FornecedorViewModel>(), "Id", "Id", fornecedorEnderecoViewModel.FornecedorId);
            return View(fornecedorEnderecoViewModel);
        }

        // GET: FornecedorEnderecos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FornecedorEnderecoViewModel == null)
            {
                return NotFound();
            }

            var fornecedorEnderecoViewModel = await _context.FornecedorEnderecoViewModel
                .Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fornecedorEnderecoViewModel == null)
            {
                return NotFound();
            }

            return View(fornecedorEnderecoViewModel);
        }

        // POST: FornecedorEnderecos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FornecedorEnderecoViewModel == null)
            {
                return Problem("Entity set 'MinhasVendasAppContext.FornecedorEnderecoViewModel'  is null.");
            }
            var fornecedorEnderecoViewModel = await _context.FornecedorEnderecoViewModel.FindAsync(id);
            if (fornecedorEnderecoViewModel != null)
            {
                _context.FornecedorEnderecoViewModel.Remove(fornecedorEnderecoViewModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorEnderecoViewModelExists(int id)
        {
          return (_context.FornecedorEnderecoViewModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
