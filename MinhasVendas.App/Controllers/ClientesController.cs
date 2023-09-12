using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers
{
    public class ClientesController : BaseController
    {
        private readonly IClienteRespositorio _clienteRespositorio;
        private readonly IClienteEnderecoRepositorio _clienteEnderecoRepositorio;
        private readonly IClienteServico _clienteServico;
        private readonly IMapper _mapper;

        public ClientesController(
                                  IClienteRespositorio clienteRespositorio,
                                  IClienteEnderecoRepositorio clienteEnderecoRepositorio,
                                  IClienteServico clienteServico,
                                  IMapper mapper,
                                  INotificador notificador) : base(notificador)  
        {
            _clienteRespositorio = clienteRespositorio;
            _clienteEnderecoRepositorio = clienteEnderecoRepositorio;
            _clienteServico = clienteServico;
            _mapper = mapper;
        }
     
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteRespositorio.Obter().ToListAsync();

            var clientesViewModel = _mapper.Map<IEnumerable<ClienteViewModel>>(clientes);

            return View(clientesViewModel);

        }

        public async Task<IActionResult> Details(int id)
        {
            var clienteEnderecoProdutos = await _clienteRespositorio.ObterClienteProdutoEndereco(id);

            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();

            var clienteViewModel = _mapper.Map<ClienteViewModel>(clienteEnderecoProdutos);
          
            return View(clienteViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid) return View(clienteViewModel);

            var cliente = _mapper.Map<Cliente>(clienteViewModel);

            await _clienteServico.Adicionar(cliente);

            if (!OperacaoValida()) return View(clienteViewModel);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();

            var clienteViewModel = _mapper.Map<ClienteViewModel>(cliente);
          
            return View(clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid) return View(clienteViewModel);

            if (id != clienteViewModel.Id) return NotFound();

            var clienteDB = await _clienteRespositorio.ObterSemRastreamento().FirstOrDefaultAsync(c=> c.Id == id);

            if (clienteDB == null) return NotFound();

            var cliente = _mapper.Map<Cliente>(clienteViewModel);

            await _clienteServico.Atualizar(cliente);

            if (!OperacaoValida()) return View(clienteViewModel);

            return RedirectToAction("Details", new { id = cliente.Id });
        
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _clienteRespositorio.ObterPorId(c => c.Id == id);
            
            if (cliente == null) return NotFound();

            var clienteViewModel = _mapper.Map<ClienteViewModel>(cliente);
           
            return View(clienteViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();

            _clienteRespositorio.Desanexar(cliente);

            await _clienteServico.Remover(id);

            var clienteViewModel = _mapper.Map<ClienteViewModel>(cliente);

            if (!OperacaoValida()) return View(clienteViewModel);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]                                
        public async Task<IActionResult> EditarEnderecoCliente(int id)
        {
            var endereco = await _clienteEnderecoRepositorio.ObterPorId(e => e.Id == id);

            if (endereco == null) return NotFound();

            var clienteEnderecoViewModel = _mapper.Map<ClienteEnderecoViewModel>(endereco);

            return PartialView("_AtualizarEnderecoCliente", clienteEnderecoViewModel);
            //return View("_AtualizarEnderecoCliente", clienteEnderecoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarEnderecoCliente(ClienteEnderecoViewModel clienteEnderecoViewModel)
        {


            if (!ModelState.IsValid) return PartialView("_AtualizarEnderecoCliente", clienteEnderecoViewModel);

            var endereco = await _clienteEnderecoRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(e => e.Id ==  clienteEnderecoViewModel.Id);

            if (endereco == null) return NotFound();

            var clienteEndereco = _mapper.Map<ClienteEndereco>(clienteEnderecoViewModel);

            await _clienteEnderecoRepositorio.Atualizar(clienteEndereco);

            if (!OperacaoValida()) return View(clienteEnderecoViewModel);

            return RedirectToAction("Details", new { id = clienteEndereco.ClienteId });

        }



    }
}
