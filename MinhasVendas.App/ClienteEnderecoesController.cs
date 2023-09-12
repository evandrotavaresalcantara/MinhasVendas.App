using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Models;

namespace MinhasVendas.App
{
    public class ClienteEnderecoesController : Controller
    {
        private readonly MinhasVendasAppContext _context;

        public ClienteEnderecoesController(MinhasVendasAppContext context)
        {
            _context = context;
        }

        // GET: ClienteEnderecoes
        public async Task<IActionResult> Index()
        {
            var minhasVendasAppContext = _context.ClienteEndereco.Include(c => c.Cliente);
            return View(await minhasVendasAppContext.ToListAsync());
        }

        // GET: ClienteEnderecoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ClienteEndereco == null)
            {
                return NotFound();
            }

            var clienteEndereco = await _context.ClienteEndereco
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clienteEndereco == null)
            {
                return NotFound();
            }

            return View(clienteEndereco);
        }

        // GET: ClienteEnderecoes/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id");
            return View();
        }

        // POST: ClienteEnderecoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Estado,Id")] ClienteEndereco clienteEndereco)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clienteEndereco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", clienteEndereco.ClienteId);
            return View(clienteEndereco);
        }

        // GET: ClienteEnderecoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ClienteEndereco == null)
            {
                return NotFound();
            }

            var clienteEndereco = await _context.ClienteEndereco.FindAsync(id);
            if (clienteEndereco == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", clienteEndereco.ClienteId);
            return View(clienteEndereco);
        }

        // POST: ClienteEnderecoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Estado,Id")] ClienteEndereco clienteEndereco)
        {
            if (id != clienteEndereco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clienteEndereco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteEnderecoExists(clienteEndereco.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", clienteEndereco.ClienteId);
            return View(clienteEndereco);
        }

        // GET: ClienteEnderecoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ClienteEndereco == null)
            {
                return NotFound();
            }

            var clienteEndereco = await _context.ClienteEndereco
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clienteEndereco == null)
            {
                return NotFound();
            }

            return View(clienteEndereco);
        }

        // POST: ClienteEnderecoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ClienteEndereco == null)
            {
                return Problem("Entity set 'MinhasVendasAppContext.ClienteEndereco'  is null.");
            }
            var clienteEndereco = await _context.ClienteEndereco.FindAsync(id);
            if (clienteEndereco != null)
            {
                _context.ClienteEndereco.Remove(clienteEndereco);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteEnderecoExists(int id)
        {
          return (_context.ClienteEndereco?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
