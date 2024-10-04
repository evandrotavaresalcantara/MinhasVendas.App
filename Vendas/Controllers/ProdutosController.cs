using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Models;
using Vendas.Models.Enums;
using Vendas.Paginacao;
using Vendas.ViewModels;

namespace Vendas.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly VendasAppContext _context;
        private readonly IProdutoServico _produtoServico;
        private readonly IMapper _mapper;
        private readonly IProdutoCategoriaRepositorio _produtoCategoriaRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutosController(VendasAppContext context,
                                  INotificador notificador,
                                  IMapper mapper,
                                  IProdutoCategoriaRepositorio produtoCategoriaRepositorio,
                                  IProdutoRepositorio produtoRepositorio,
                                  IProdutoServico produtoServico) : base(notificador)
        {
            _context = context;
            _produtoServico = produtoServico;
            _produtoRepositorio = produtoRepositorio;
            _produtoCategoriaRepositorio = produtoCategoriaRepositorio;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoViewModel>> Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObterProdutos(int draw, int start, int length, int statusFiltro,
                                             [FromQuery(Name = "search[value]")] string search,
                                             [FromQuery(Name = "order[0][column]")] int ordenacao,
                                             [FromQuery(Name = "order[0][dir]")] string direcao)
        {

            var parametros = new ProdutosParametros();

            parametros.draw = draw;
            parametros.start = start;
            parametros.lenght = length;
            parametros.search = search;
            parametros.Filtro = statusFiltro;
            parametros.Ordenacao = ordenacao;
            parametros.Direcao = direcao;

            string json = await _produtoServico.ObterProdutos(parametros);

            return Content(json, "application/json");
        }

        public async Task<IActionResult> Obter(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }


            var produtoViewModel = _mapper.Map<ProdutoViewModel>(produto);

            var saldoProduto = _context.TransacaoDeEstoques
                                .Where(te => te.ProdutoId == id)
                                .Sum(te => te.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra ? te.Quantidade : -te.Quantidade);

            produtoViewModel.EstoqueAtual = saldoProduto;
            return View(produtoViewModel);
        }

        public IActionResult Novo()
        {
            ViewData["CategoriaId"] = new SelectList(_produtoCategoriaRepositorio.Obter(), "Id", "Nome");

            return View();


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo(ProdutoViewModel produtoViewModel)
        {
            ViewData["CategoriaId"] = new SelectList(_produtoCategoriaRepositorio.Obter(), "Id", "Nome");

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "-";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }
                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            var produto = _mapper.Map<Produto>(produtoViewModel);

            await _produtoServico.Adicionar(produto);

            if (!OperacaoValida())
                return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Atualizar(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            ViewData["CategoriaId"] = new SelectList(_produtoCategoriaRepositorio.Obter(), "Id", "Nome");

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            var produtoViewModel = _mapper.Map<ProdutoViewModel>(produto);

            return View(produtoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(int id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id) return NotFound();

            var produtoDB = await _produtoRepositorio.ObterSemRastreamento().FirstOrDefaultAsync(p => p.Id == id);

            if (produtoDB == null) return NotFound();

            produtoViewModel.Imagem = produtoDB.Imagem;

            ViewData["CategoriaId"] = new SelectList(_produtoCategoriaRepositorio.Obter(), "Id", "Nome");

            if (!ModelState.IsValid) return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                var imgPrefixo = Guid.NewGuid() + "-";
                if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
                {
                    return View(produtoViewModel);
                }

                produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;
            }

            var produto = _mapper.Map<Produto>(produtoViewModel);

            await _produtoServico.Atualizar(produto);

            if (!OperacaoValida())
            {
                produtoViewModel.Imagem = produtoDB.Imagem;
                return View(produtoViewModel);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Excluir(int? id)
        {
            if (id == null || _context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            var produtoViewModel = _mapper.Map<ProdutoViewModel>(produto);

            var saldoProduto = _context.TransacaoDeEstoques
                   .Where(te => te.ProdutoId == id)
                   .Sum(te => te.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra ? te.Quantidade : -te.Quantidade);

            produtoViewModel.EstoqueAtual = saldoProduto;

            return View(produtoViewModel);
        }

        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcluisaoConfirmada(int id)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entity set 'AppEstoquesEVendasContext.Produtos'  is null.");
            }
            var produto = await _context.Produtos.FindAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagensProdutos", imgPrefixo + arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

    }
}
