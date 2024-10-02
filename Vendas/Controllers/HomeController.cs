using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Servicos;
using Vendas.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Vendas.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly VendasAppContext _minhasVendasAppContext;
        private readonly IMapper _mapper;
        private readonly INotificador _notificadorInfo;
        private readonly IClienteRespositorio _clienteRepositorio;
        private readonly IClienteServico _clienteServico;
        private readonly IClienteEnderecoRepositorio _clienteEnderecoRepositorio;
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IFornecedorServico _fornecedorServico;
        private readonly IFornecedorEnderecoRepositorio _fornecedorEnderecoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IProdutoServico _produtoServico;
        private readonly IProdutoCategoriaRepositorio _produtoCategoriaRepositorio;
        private readonly IOrdemDeCompraServico _ordemDeCompraServico;
        private readonly IOrdemDeCompraRepositorio _ordemDeCompraRepositorio;
        private readonly IOrdemDeVendaRepositorio _ordemDeVendaRepositorio;
        private readonly IOrdemDeVendaServico _ordemDeVendaServico;


        public HomeController(
                                  VendasAppContext minhasVendasAppContext,
                                  IConfiguration configuration,
                                  IClienteRespositorio clienteRepositorio,
                                  IClienteEnderecoRepositorio clienteEnderecoRepositorio,
                                  IClienteServico clienteServico,
                                  IMapper mapper,
                                  INotificador notificadorInfo,
                                  IFornecedorRepositorio fornecedorRepositorio,
                                  IFornecedorServico fornecedorServico,
                                  IFornecedorEnderecoRepositorio fornecedorEnderecoRepositorio,
                                  IProdutoRepositorio produtoRepositorio,
                                  IProdutoServico produtoServico,
                                  IProdutoCategoriaRepositorio produtoCategoriaRepositorio,
                                  IOrdemDeCompraRepositorio ordemDeCompraRepositorio,
                                  IOrdemDeCompraServico ordemDeCompraServico,
                                  IOrdemDeVendaRepositorio ordemDeVendaRepositorio,
                                  IOrdemDeVendaServico ordemDeVendaServico,

                                  INotificador notificador) : base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
            _configuration = configuration;
            _clienteRepositorio = clienteRepositorio;
            _clienteEnderecoRepositorio = clienteEnderecoRepositorio;
            _clienteServico = clienteServico;
            _mapper = mapper;
            _notificadorInfo = notificadorInfo;
            _fornecedorRepositorio = fornecedorRepositorio;
            _fornecedorServico = fornecedorServico;
            _fornecedorEnderecoRepositorio = fornecedorEnderecoRepositorio;
            _produtoRepositorio = produtoRepositorio;
            _produtoServico = produtoServico;
            _produtoCategoriaRepositorio = produtoCategoriaRepositorio;
            _ordemDeCompraRepositorio = ordemDeCompraRepositorio;
            _ordemDeCompraServico = ordemDeCompraServico;
            _ordemDeVendaRepositorio = ordemDeVendaRepositorio;
            _ordemDeVendaServico = ordemDeVendaServico;
        }

        [HttpGet]
        public IActionResult Exemplo()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Exemplo(string email)
        {

            return Content($"Email recebido : {email}");

        }


        public IActionResult Index()
        {

            ViewBag.TotalClientes = _clienteEnderecoRepositorio.Obter().Count();
            ViewBag.TotalFornecedores = _fornecedorEnderecoRepositorio.Obter().Count();
            ViewBag.TotalProdutos = _produtoRepositorio.Obter().Count();
            ViewBag.TotalOrdemDeCompras = _ordemDeCompraRepositorio.Obter().Count();
            ViewBag.TotalOrdemDeVendas = _ordemDeVendaRepositorio.Obter().Count();

            string nomeImagem = "img.png";
            var pathImagensProdutosTmp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagensProdutosTmp", nomeImagem);

            ViewBag.Path = pathImagensProdutosTmp;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarClientes(int excluir, int quantidade)
        {
            if (excluir == 1)
            {
                if (_clienteRepositorio.Obter().Any())
                {
                    var clientesDB = _clienteRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(clientesDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todos os clientes foram Removidos.";

                    return RedirectToAction("index", "home");
                }
            }


            ClientesController clienteController = new ClientesController(_clienteRepositorio, _clienteEnderecoRepositorio, _clienteServico, _mapper, _notificadorInfo);
            ClienteViewModel clienteViewModel = new ClienteViewModel();
            ClienteEnderecoViewModel clienteEnderecoViewModel = new ClienteEnderecoViewModel();

            var prefixo = "info";

            for (int i = 1; i <= quantidade; i++)
            {
                if (i < 10)
                {

                    clienteViewModel.Nome = $"0{i}-{prefixo}";
                    clienteViewModel.SobreNome = $"0{i}-{prefixo}";
                    clienteViewModel.DataNascimento = DateOnly.MaxValue;
                    clienteViewModel.Celular = $"0{i}-{prefixo}";
                    clienteViewModel.WhatsApp = $"0{i}-{prefixo}";
                    clienteViewModel.Instagram = $"0{i}-{prefixo}";
                    clienteViewModel.Email = $"0{i}-{prefixo}";


                    //clienteEnderecoViewModel.ClienteId = _clienteRespositorio.Obter().FirstOrDefault().Id;
                    clienteEnderecoViewModel.Cep = "53030100";
                    clienteEnderecoViewModel.Logradouro = "Rua Coronel Henrique Guimarães";
                    clienteEnderecoViewModel.Numero = $"{prefixo}0{i}";
                    clienteEnderecoViewModel.Complemento = $"{prefixo}0{i}";
                    clienteEnderecoViewModel.Bairro = "Bairro Novo";
                    clienteEnderecoViewModel.Cidade = "Olinda";
                    clienteEnderecoViewModel.Estado = "PE";

                }
                else
                {
                    clienteViewModel.Nome = $"{i}-{prefixo}";
                    clienteViewModel.SobreNome = $"{i}-{prefixo}";
                    clienteViewModel.DataNascimento = DateOnly.MaxValue;
                    clienteViewModel.Celular = $"{i}-{prefixo}";
                    clienteViewModel.WhatsApp = $"{i}-{prefixo}";
                    clienteViewModel.Instagram = $"{i}-{prefixo}";
                    clienteViewModel.Email = $"{i}-{prefixo}";

                    //clienteEnderecoViewModel.ClienteId = _clienteRespositorio.Obter().FirstOrDefault().Id;
                    clienteEnderecoViewModel.Cep = "53030100";
                    clienteEnderecoViewModel.Logradouro = "Rua Coronel Henrique Guimarães";
                    clienteEnderecoViewModel.Numero = $"{prefixo}0{i}";
                    clienteEnderecoViewModel.Complemento = $"{prefixo}0{i}";
                    clienteEnderecoViewModel.Bairro = "Bairro Novo";
                    clienteEnderecoViewModel.Cidade = "Olinda";
                    clienteEnderecoViewModel.Estado = "PE";

                }

                clienteViewModel.Endereco = clienteEnderecoViewModel;
                await clienteController.Novo(clienteViewModel);
            }


            if (!OperacaoValida()) return View();

            ViewBag.Resultado = "Dados Carregado com sucesso!";

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarFornecedores(int excluir, int quantidade)
        {

            if (excluir == 1)
            {
                if (_fornecedorRepositorio.Obter().Any())
                {
                    var fornecedoresDB = _fornecedorRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(fornecedoresDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todos os fornecedores foram Removidos.";

                    return RedirectToAction("index", "home");
                }
            }

            FornecedoresController fornecedorController = new FornecedoresController(_fornecedorRepositorio, _fornecedorEnderecoRepositorio, _fornecedorServico, _mapper, _notificadorInfo);
            FornecedorViewModel fornecedorViewModel = new FornecedorViewModel();
            FornecedorEnderecoViewModel fornecedorEnderecoViewModel = new FornecedorEnderecoViewModel();

            var prefixo = "info";

            for (int i = 1; i <= quantidade; i++)
            {
                if (i < 10)
                {

                    fornecedorViewModel.Documento = $"0{i}-{prefixo}";
                    fornecedorViewModel.Nome = $"0{i}-{prefixo}";
                    fornecedorViewModel.TipoFornecedor = i % 2 == 0 ? 2 : 1;
                    fornecedorViewModel.Celular = $"0{i}-{prefixo}";
                    fornecedorViewModel.WhatsApp = $"0{i}-{prefixo}";
                    fornecedorViewModel.Instagram = $"0{i}-{prefixo}";
                    fornecedorViewModel.Email = $"0{i}-{prefixo}";


                    fornecedorEnderecoViewModel.Cep = "53030100";
                    fornecedorEnderecoViewModel.Logradouro = "Rua Coronel Henrique Guimarães";
                    fornecedorEnderecoViewModel.Numero = $"{prefixo}0{i}";
                    fornecedorEnderecoViewModel.Complemento = $"{prefixo}0{i}";
                    fornecedorEnderecoViewModel.Bairro = "Bairro Novo";
                    fornecedorEnderecoViewModel.Cidade = "Olinda";
                    fornecedorEnderecoViewModel.Estado = "PE";

                }
                else
                {

                    fornecedorViewModel.Documento = $"{i}-{prefixo}";
                    fornecedorViewModel.Nome = $"{i}-{prefixo}";
                    fornecedorViewModel.TipoFornecedor = i % 2 == 0 ? 2 : 1;
                    fornecedorViewModel.Celular = $"{i}-{prefixo}";
                    fornecedorViewModel.WhatsApp = $"{i}-{prefixo}";
                    fornecedorViewModel.Instagram = $"{i}-{prefixo}";
                    fornecedorViewModel.Email = $"{i}-{prefixo}";


                    fornecedorEnderecoViewModel.Cep = "53030100";
                    fornecedorEnderecoViewModel.Logradouro = "Rua Coronel Henrique Guimarães";
                    fornecedorEnderecoViewModel.Numero = $"{prefixo}0{i}";
                    fornecedorEnderecoViewModel.Complemento = $"{prefixo}0{i}";
                    fornecedorEnderecoViewModel.Bairro = "Bairro Novo";
                    fornecedorEnderecoViewModel.Cidade = "Olinda";
                    fornecedorEnderecoViewModel.Estado = "PE";

                }

                fornecedorViewModel.Endereco = fornecedorEnderecoViewModel;
                await fornecedorController.Novo(fornecedorViewModel);
            }


            if (!OperacaoValida()) return View();

            ViewBag.Resultado = "Dados Carregado com sucesso!";

            return RedirectToAction("index", "home");
        }



        [HttpPost]
        public async Task<IActionResult> GerenciarProdutos(int excluir, int quantidade)
        {

            if (excluir == 1)
            {
                if (_produtoRepositorio.Obter().Any())
                {
                    var produtosDB = _produtoRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(produtosDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todos os produtos foram Removidos.";




                    var pathImagensProdutosTmp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagensProdutosTmp");
                    var pathImagensProdutos = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagensProdutos");

                    DirectoryInfo dirProdutosTmp = new(pathImagensProdutosTmp);
                    DirectoryInfo dirProdutos = new(pathImagensProdutos);

                    foreach (FileInfo file1 in dirProdutosTmp.GetFiles())
                    {
                         file1.Delete();
                    }

                    foreach (FileInfo file2 in dirProdutos.GetFiles())
                    {
                        file2.Delete();
                    }


                    if (!OperacaoValida()) return View("Resultado");

                    ViewBag.Resultado = "Produtos excluídos com sucesso!";


                    return RedirectToAction("index", "home");

                }
            }


            ProdutosController produtoController = new ProdutosController(_minhasVendasAppContext, _notificadorInfo, _mapper, _produtoCategoriaRepositorio, _produtoRepositorio, _produtoServico);
            ProdutoViewModel produtoViewModel = new ProdutoViewModel();

            var prefixo = "nomeProduto";

            for (int i = 1; i <= quantidade; i++)
            {


                produtoViewModel.ProdutoCategoriaId = 1;
                produtoViewModel.Nome = $"{i}-{prefixo}";
                produtoViewModel.Codigo = $"{i}-{prefixo}";
                produtoViewModel.Descricao = $"{i}-{prefixo}";
                produtoViewModel.Ativo = true;
                produtoViewModel.DataDeCadastro = DateTime.Now;
                produtoViewModel.PrecoDeCusto = i;
                produtoViewModel.PrecoDeVenda = i * 2;
                GeracaoImagem.GerarImagem($"{i}-{prefixo}", $"{i}-{prefixo}");

                string nomeImagem = $"{i}-{prefixo}.png";
                var pathImagensProdutosTmp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagensProdutosTmp", nomeImagem);

                using (var fluxoImagemCriada = new FileStream(pathImagensProdutosTmp, FileMode.Open))
                {

                    var imagemRecebida = new MemoryStream();
                    await fluxoImagemCriada.CopyToAsync(imagemRecebida);

                    var meuFormFile = new FormFile(imagemRecebida, 0, imagemRecebida.Length, "name", nomeImagem.Remove(0, 1));

                    produtoViewModel.ImagemUpload = meuFormFile;


                }
                await produtoController.Novo(produtoViewModel);
            }


            if (!OperacaoValida()) return View("Resultado");

            ViewBag.Resultado = "Dados Carregado com sucesso!";


            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarOrdemDeCompras(int excluir, int quantidade)
        {

            if (excluir == 1)
            {
                if (_ordemDeCompraRepositorio.Obter().Any())
                {
                    var ordemDeComprasDB = _ordemDeCompraRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(ordemDeComprasDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todas as ordens de Compras foram Removidas.";

                    return RedirectToAction("index", "home");
                }
            }


            OrdemDeComprasController ordemDeComprasController = new(_ordemDeCompraServico, _fornecedorRepositorio, _ordemDeCompraRepositorio, _mapper, _notificadorInfo);
            OrdemDeCompraViewModel ordemDeCompraViewModel = new();
            ProdutoViewModel produtoViewModel = new();
            FornecedorViewModel fornecedorViewModel = new FornecedorViewModel();

            var prefixo = "nomeProduto";

            for (int i = 1; i <= quantidade; i++)
            {
                ordemDeCompraViewModel.FornecedorId = _fornecedorRepositorio.Obter().FirstOrDefault().Id;

                await ordemDeComprasController.Novo(ordemDeCompraViewModel);
            }


            if (!OperacaoValida()) return View("Resultado");

            ViewBag.Resultado = "Dados Carregado com sucesso!";


            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarOrdemDeVendas(int excluir, int quantidade)
        {

            if (excluir == 1)
            {
                if (_ordemDeVendaRepositorio.Obter().Any())
                {
                    var ordemDeVendasDB = _ordemDeVendaRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(ordemDeVendasDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todas as ordens de Vendas foram Removidas.";

                    return RedirectToAction("index", "home");
                }
            }


            OrdemDeVendasController ordemDeVendasController = new(_minhasVendasAppContext, _ordemDeVendaServico, _clienteRepositorio, _ordemDeVendaRepositorio, _mapper, _notificadorInfo);
            OrdemDeVendaViewModel ordemDeVendaViewModel = new();
            ProdutoViewModel produtoViewModel = new();
            ClienteViewModel fornecedorViewModel = new();


            for (int i = 1; i <= quantidade; i++)
            {
                ordemDeVendaViewModel.ClienteId = _clienteRepositorio.Obter().FirstOrDefault().Id;

                await ordemDeVendasController.Novo(ordemDeVendaViewModel);
            }


            if (!OperacaoValida()) return View("Resultado");

            ViewBag.Resultado = "Dados Carregado com sucesso!";


            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarProdutoCategoria(int excluir, int quantidade)
        {

            if (excluir == 1)
            {
                if (_produtoCategoriaRepositorio.Obter().Any())
                {
                    var produtoCategoriasDB = _produtoCategoriaRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(produtoCategoriasDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todas as categorias foram Removidas.";

                    return RedirectToAction("index", "home");
                }
            }


            await _produtoCategoriaRepositorio.Adicionar(new Models.ProdutoCategoria { Nome = "Categoria", Descricao = "Descricao", Produtos = null });
           

            ViewBag.Resultado = "Dados Carregado com sucesso!";


            return RedirectToAction("index", "home");
        }
    }
}