using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vendas.Data;
using Vendas.Interfaces;
using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Servicos;
using Vendas.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Vendas.Models.Enums;

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

        public IActionResult Index()
        {
            var resumo = ObterResumoCadastro();

            return View(resumo);
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarClientes(ResumoCadastro model)
        {
            var validarQuantidade = ValidarQuantidadeResumoCadastro(model);
            var resumo = ObterResumoCadastro();

            if (!validarQuantidade) return View("index", resumo);

            var excluir = model.Excluir;
            var quantidade = model.Quantidade;

            var existeTransacaoVenda = _minhasVendasAppContext.TransacaoDeEstoques.Any(v => v.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Venda);
            if (existeTransacaoVenda)
            {
                Notificar("Existem transações de venda cadastrada. Primeiro exclua produtos e tente novamente.");
                return View("index", resumo);
            }


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
        public async Task<IActionResult> GerenciarFornecedores(ResumoCadastro model)
        {
            var validarQuantidade = ValidarQuantidadeResumoCadastro(model);
            var resumo = ObterResumoCadastro();

            if (!validarQuantidade) return View("index", resumo);

            var existeTransacaoCompra = _minhasVendasAppContext.TransacaoDeEstoques.Any(v => v.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra);
            if (existeTransacaoCompra)
            {
                Notificar("Existem transações de compra cadastrada. Primeiro exclua produtos e tente novamente.");
                return View("index", resumo);
            }

            var excluir = model.Excluir;
            var quantidade = model.Quantidade;
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
        public async Task<IActionResult> GerenciarProdutos(ResumoCadastro model)
        {
            var validarQuantidade = ValidarQuantidadeResumoCadastro(model);
            var resumo = ObterResumoCadastro();

            if(!validarQuantidade) return View("index", resumo);
            
            var excluir = model.Excluir;
            var quantidade = model.Quantidade;

            if (excluir == 1)
            {
                if (_produtoRepositorio.Obter().Any())
                {
                    var produtosDB = _produtoRepositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(produtosDB);
                    _minhasVendasAppContext.SaveChanges();

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

                    if (!OperacaoValida()) return View("index", resumo);

                    var resumoAtualizado1 = ObterResumoCadastro();
                    return View("index", resumoAtualizado1 );
                }
            }

            ProdutosController produtoController = new ProdutosController(_minhasVendasAppContext, _notificadorInfo, _mapper, _produtoCategoriaRepositorio, _produtoRepositorio, _produtoServico);
            ProdutoViewModel produtoViewModel = new ProdutoViewModel();

            var prefixo = "nomeProduto";
                      
            for (decimal i = 1; i <= quantidade; i++)
            {
                var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                produtoViewModel.ProdutoCategoriaId = 1;
                produtoViewModel.Nome = $"{timestamp}-{prefixo}";
                produtoViewModel.Codigo = $"{i}-{prefixo}";
                produtoViewModel.Descricao = $"{i}-{prefixo}";
                produtoViewModel.Ativo = true;
                produtoViewModel.DataDeCadastro = DateTime.Now;
                produtoViewModel.PrecoDeCusto = i * 1.00m;
                produtoViewModel.PrecoDeVenda = i * 1.00m + 20;
                produtoViewModel.Imagem = $"{timestamp}";

                await produtoController.Novo(produtoViewModel);

                if (!OperacaoValida()) return View("index", resumo );
            }
            var resumoAtualizado = ObterResumoCadastro();
            return View("index", resumoAtualizado);
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarOrdemDeCompras(ResumoCadastro model)
        {
            var validarQuantidade = ValidarQuantidadeResumoCadastro(model);
            var resumo = ObterResumoCadastro();

            if (!validarQuantidade) return View("index", resumo);

            var existeTransacaoVenda = _minhasVendasAppContext.TransacaoDeEstoques.Any(v => v.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Compra);
            if (existeTransacaoVenda)
            {
                Notificar("Existe transações de compra cadastrada. Primeiro exclua produtos e tente novamente.");
                return View("index", resumo);
            }

            var excluir = model.Excluir;
            var quantidade = model.Quantidade;

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

            var existeFornecedor = _minhasVendasAppContext.Fornecedores.Any();
            if (!existeFornecedor)
            {
                Notificar("Não existe nenhum fornecedor cadastrado. Primeiro crie ao menos um fornecedor.");
                return View("index", resumo);
            }

            var prefixo = "nomeProduto";

            for (int i = 1; i <= quantidade; i++)
            {
                ordemDeCompraViewModel.FornecedorId = _fornecedorRepositorio.Obter().FirstOrDefault().Id;

                await ordemDeComprasController.Novo(ordemDeCompraViewModel);
            }

            if (!OperacaoValida()) return View("index", resumo);

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarOrdemDeVendas(ResumoCadastro model)
        {
            var validarQuantidade = ValidarQuantidadeResumoCadastro(model);
            var resumo = ObterResumoCadastro();

            if (!validarQuantidade) return View("index", resumo);

            var existeTransacaoVenda = _minhasVendasAppContext.TransacaoDeEstoques.Any(v => v.TipoDransacaoDeEstoque == TipoDransacaoDeEstoque.Venda);
            if (existeTransacaoVenda)
            {
                Notificar("Existem transações de venda cadastrada. Primeiro exclua produtos e tente novamente.");
                return View("index", resumo);
            }

            var excluir = model.Excluir;
            var quantidade = model.Quantidade;

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

            var existeCliente = _minhasVendasAppContext.Clientes.Any();
            if (!existeCliente)
            {
                Notificar("Não existe nenhum cliente cadastrado. Primeiro crie ao menos um cliente.");
                return View("index", resumo);
            }

            for (int i = 1; i <= quantidade; i++)
            {
                ordemDeVendaViewModel.ClienteId = _clienteRepositorio.Obter().FirstOrDefault().Id;

                await ordemDeVendasController.Novo(ordemDeVendaViewModel);
            }


            if (!OperacaoValida()) return View("index", resumo);

            return RedirectToAction("index", "home");
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarProdutoCategoria(ResumoCadastro model)
        {
            var validarQuantidade = ValidarQuantidadeResumoCadastro(model);
            var resumo = ObterResumoCadastro();

            if (!validarQuantidade) return View("index", resumo);

            var excluir = model.Excluir;
            var quantidade = model.Quantidade;

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

        ResumoCadastro  ObterResumoCadastro()
        {
            ResumoCadastro resumoCadastro = new()
            {
                TotalClientes = _clienteEnderecoRepositorio.Obter().Count(),
                TotalFornecedores = _fornecedorEnderecoRepositorio.Obter().Count(),
                TotalProdutos = _produtoRepositorio.Obter().Count(),
                TotalOrdemDeCompras = _ordemDeCompraRepositorio.Obter().Count(),
                TotalOrdemDeVendas = _ordemDeVendaRepositorio.Obter().Count()
            };

            return resumoCadastro;
        }

        bool ValidarQuantidadeResumoCadastro(ResumoCadastro model)
        {

            if (model.Quantidade > 1000 || model.Quantidade < 0)
            {
                Notificar("# Quantidade não pode ser maior que 1000 ou menor que 0.");

                return false;
            }
            return true;
        }
    }

    public class ResumoCadastro
    {
        public int TotalClientes { get; set; }
        public int TotalFornecedores { get; set; }
        public int TotalProdutos { get; set; }
        public int TotalOrdemDeCompras { get; set; }
        public int TotalOrdemDeVendas { get; set; }
        public int Excluir { get; set; }
        public int Quantidade { get; set; }
    }
}