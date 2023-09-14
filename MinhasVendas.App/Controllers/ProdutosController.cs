using System;
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
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Repositorio;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.Controllers
{
    public class ProdutosController : BaseController
    {
        private readonly MinhasVendasAppContext _context;
        private readonly IProdutoServico _produtoServico;
        private readonly IMapper _mapper;
        private readonly IProdutoCategoriaRepositorio _produtoCategoriaRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutosController(MinhasVendasAppContext context,
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
        public ActionResult<IEnumerable<ProdutoViewModel>> Index(string ordemDeClassificacao, string filtroAtual, string pesquisarTexto, int? numeroDePagina)
        {
            var produtosParametros = new ProdutosParametros() { NumeroDePagina = numeroDePagina ?? 1, TamanhoDePagina = 10 };

            ViewData["ClassificacaoAtual"] = ordemDeClassificacao;
            ViewData["NomeClassificarParam"] = String.IsNullOrEmpty(ordemDeClassificacao) ? "nome_descendente" : "";
            ViewData["CodigoClassificarParam"] = ordemDeClassificacao == "codigo" ? "codigo_descendente" : "codigo";


            produtosParametros.OrdemDeClassificacao = ordemDeClassificacao;
            produtosParametros.PesquisaTexto = pesquisarTexto;
            produtosParametros.FiltroAtual = filtroAtual;

            ViewData["FiltroAtual"] = produtosParametros.PesquisaTexto ?? produtosParametros.FiltroAtual;


            var qtdCompraEVenda =
                   from produtoQtd in _context.TransacaoDeEstoques
                   group produtoQtd by produtoQtd.ProdutoId into produtoGroup
                   select new
                   {
                       produtoGroup.Key,
                       totalProdutoComprado = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra).Sum(p => p.Quantidade),
                       totalProdutoVendido = produtoGroup.Where(p => p.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Venda).Sum(p => p.Quantidade),
                   };


            var listaPaginadaProdutos = _produtoRepositorio.ObterProdutosPaginacaoLista(produtosParametros);

            var metadata = new
            {
                listaPaginadaProdutos.TotalDeItens,
                listaPaginadaProdutos.TamanhoDaPagina,
                listaPaginadaProdutos.PaginaAtual,
                listaPaginadaProdutos.TotalDePaginas,
                listaPaginadaProdutos.TemProxima,
                listaPaginadaProdutos.TemAnterior

            };


            foreach (var produto in listaPaginadaProdutos)
            {
                var item = qtdCompraEVenda.FirstOrDefault(p => p.Key == produto.Id);

                if (item == null) continue;

                produto.EstoqueAtual = item.totalProdutoComprado - item.totalProdutoVendido;
            }



            ViewBag.Metada = metadata;

            var produtosViewModel = _mapper.Map<IEnumerable<ProdutoViewModel>>(listaPaginadaProdutos);

            return View(produtosViewModel);
        }




        // GET: Produtos/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(produtoViewModel);
        }

        // GET: Produtos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_produtoCategoriaRepositorio.Obter(), "Id", "Nome");

            return View();


        }

        // POST: Produtos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            ViewData["CategoriaId"] = new SelectList(_produtoCategoriaRepositorio.Obter(), "Id", "Nome");
            if (!ModelState.IsValid) return View(produtoViewModel);

            var imgPrefixo = Guid.NewGuid() + "-";
            if (!await UploadArquivo(produtoViewModel.ImagemUpload, imgPrefixo))
            {
                return View(produtoViewModel);
            }

            produtoViewModel.Imagem = imgPrefixo + produtoViewModel.ImagemUpload.FileName;

            var produto = _mapper.Map<Produto>(produtoViewModel);

            await _produtoServico.Adicionar(produto);

            if (!OperacaoValida()) return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        // GET: Produtos/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Produtos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProdutoViewModel produtoViewModel)
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

        // GET: Produtos/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            return View(produtoViewModel);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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

        private bool ProdutoExists(int id)
        {
            return (_context.Produtos?.Any(e => e.Id == id)).GetValueOrDefault();
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
