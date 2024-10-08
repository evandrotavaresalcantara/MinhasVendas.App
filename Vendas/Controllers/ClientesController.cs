﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Paginacao;
using Vendas.ViewModels;

namespace Vendas.Controllers
{
    [Authorize]
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

        [HttpGet]
        public ActionResult<IEnumerable<OrdemDeCompraViewModel>> Index()
        {
            return View();

        }

        public async Task<IActionResult> ObterClientes(int draw, int start, int length, string statusFiltro,
                                                    [FromQuery(Name = "search[value]")] string search,
                                                    [FromQuery(Name = "order[0][column]")] int ordenacao,
                                                    [FromQuery(Name = "order[0][dir]")] string direcao)
        {

            var parametros = new ClientesParametros();

            parametros.draw = draw;
            parametros.start = start;
            parametros.lenght = length;
            parametros.search = search;
            parametros.Filtro = statusFiltro;
            parametros.Ordenacao = ordenacao;
            parametros.Direcao = direcao;

            string json = await _clienteServico.ObterClientes(parametros);

            return Content(json, "application/json");
        }
        [HttpGet]
        public async Task<IActionResult> Obter(int id)
        {
            var clienteEnderecoProdutos = await _clienteRespositorio.ObterClienteProdutoEndereco(id);

            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();

            var clienteViewModel = _mapper.Map<ClienteViewModel>(clienteEnderecoProdutos);

            return View(clienteViewModel);
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo(ClienteViewModel clienteViewModel)
        {
            if (!ModelState.IsValid) return View(clienteViewModel);

            var cliente = _mapper.Map<Cliente>(clienteViewModel);

            await _clienteServico.Adicionar(cliente);

            if (!OperacaoValida()) return View(clienteViewModel);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Atualizar(int id)
        {
            var cliente = await _clienteRespositorio.BuscarPorId(id);

            if (cliente == null) return NotFound();

            var clienteViewModel = _mapper.Map<ClienteViewModel>(cliente);

            return View(clienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(int id, ClienteViewModel clienteViewModel)
        {
            if (id != clienteViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(clienteViewModel);

            var clienteDB = await _clienteRespositorio.ObterSemRastreamento().FirstOrDefaultAsync(c => c.Id == id);

            if (clienteDB == null) return NotFound();

            var cliente = _mapper.Map<Cliente>(clienteViewModel);

            await _clienteServico.Atualizar(cliente);

            if (!OperacaoValida()) return View(clienteViewModel);

            return RedirectToAction("Obter", new { id = cliente.Id });

        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            var cliente = await _clienteRespositorio.ObterPorId(c => c.Id == id);

            if (cliente == null) return NotFound();

            var clienteViewModel = _mapper.Map<ClienteViewModel>(cliente);

            return View(clienteViewModel);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
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
        }

        [HttpPost]
        public async Task<IActionResult> EditarEnderecoCliente(ClienteEnderecoViewModel clienteEnderecoViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_AtualizarEnderecoCliente", clienteEnderecoViewModel);

            var endereco = await _clienteEnderecoRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(e => e.Id == clienteEnderecoViewModel.Id);

            if (endereco == null) return NotFound();

            var clienteEndereco = _mapper.Map<ClienteEndereco>(clienteEnderecoViewModel);

            await _clienteEnderecoRepositorio.Atualizar(clienteEndereco);

            if (!OperacaoValida()) return View(clienteEnderecoViewModel);

            return RedirectToAction("Obter", new { id = clienteEndereco.ClienteId });

        }

        [HttpGet]
        public async Task<IActionResult> ObterClientesNomeEId()
        {
            var clientes = await _clienteRespositorio.BuscarTodos();

            var clientesAutocomplete = clientes.Select(c => new
            {
                label = c.Nome,
                value = c.Id
            });

            return Json(clientesAutocomplete);
        }

        [HttpGet]
        public async Task<IActionResult> ObterClientesNome(string termo)
        {
            var clientes = await _clienteRespositorio.Buscar(c => c.Nome.Contains(termo));

            var clientesAutocomplete = clientes.Select(c => new
            {
                label = c.Nome,
                value = c.Id
            });

            return Json(clientesAutocomplete);
        }

    }
}
