using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Servicos;
using MinhasVendas.App.ViewModels;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.NetworkInformation;
using System.Linq;

namespace MinhasVendas.App.Controllers
{
    public class InfoController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly MinhasVendasAppContext _minhasVendasAppContext;
        private readonly IMapper _mapper;
        private readonly INotificador _notificadorInfo;
        private readonly IClienteRespositorio _clienteRespositorio;
        private readonly IClienteServico _clienteServico;
        private readonly IClienteEnderecoRepositorio _clienteEnderecoRepositorio;
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IFornecedorServico _fornecedorServico;
        private readonly IFornecedorEnderecoRepositorio _fornecedorEnderecoRepositorio;
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IProdutoServico _produtoServico;
        private readonly IProdutoCategoriaRepositorio _produtoCategoriaRepositorio;


        public InfoController(
                                  MinhasVendasAppContext minhasVendasAppContext,
                                  IConfiguration configuration,
                                  IClienteRespositorio clienteRespositorio,
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
                                  INotificador notificador) : base(notificador)
        {
            _minhasVendasAppContext = minhasVendasAppContext;
            _configuration = configuration;
            _clienteRespositorio = clienteRespositorio;
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
            var nome = _configuration["Info:Nome"];
            var conexao = _configuration.GetConnectionString("MinhaConexao");

            var info = new
            {
                appsettings_info_Nome = nome,
                appsettings_ConnectionStrings_MinhaConexao = conexao
            };
            ViewBag.Info = info;

            ViewBag.TotalClientes = _clienteEnderecoRepositorio.Obter().Count();
            ViewBag.TotalFornecedores = _fornecedorEnderecoRepositorio.Obter().Count();
            ViewBag.TotalProdutos = _produtoRepositorio.Obter().Count();


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GerenciarClientes(int excluir, int quantidade)
        {

            if (excluir == 1)
            {
                if (_clienteRespositorio.Obter().Any())
                {
                    var clientesDB = _clienteRespositorio.Obter().ToList();

                    _minhasVendasAppContext.RemoveRange(clientesDB);
                    _minhasVendasAppContext.SaveChanges();

                    ViewBag.Resultado = "Todos os clientes foram Removidos.";

                    return View("Resultado");
                }
            }


            ClientesController clienteController = new ClientesController(_clienteRespositorio, _clienteEnderecoRepositorio, _clienteServico, _mapper, _notificadorInfo);
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
                await clienteController.Create(clienteViewModel);
            }


            if (!OperacaoValida()) return View();

            ViewBag.Resultado = "Dados Carregado com sucesso!";

            return View("Resultado");
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

                    return View("Resultado");
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
                    fornecedorViewModel.TipoFornecedor = i % 2 == 0 ? TipoFornecedor.PessoaJuridica : TipoFornecedor.PessoaFisica;
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
                    fornecedorViewModel.TipoFornecedor = i % 2 == 0 ? TipoFornecedor.PessoaJuridica : TipoFornecedor.PessoaFisica;
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
                await fornecedorController.Create(fornecedorViewModel);
            }


            if (!OperacaoValida()) return View();

            ViewBag.Resultado = "Dados Carregado com sucesso!";

            return View("Resultado");
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


                    string pathImagensProdutosTmp = @"C:\apps\MinhasVendas\MinhasVendas.App\wwwroot\imagensProdutosTmp";
                    string pathImagensProdutos = @"C:\apps\MinhasVendas\MinhasVendas.App\wwwroot\imagensProdutos";



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






                    return View("Resultado");
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
                produtoViewModel.PrecoBase = i;
                produtoViewModel.PrecoDeLista = i * 2;
                GeracaoImagem.GerarImagem($"{i}-{prefixo}", $"{i}-{prefixo}");

                string diretorio = @"C:\apps\MinhasVendas\MinhasVendas.App\wwwroot\imagensProdutosTmp";
                string nomeImagem = $"\\{i}-{prefixo}.png";
                string imagemCriadaUpload = diretorio + nomeImagem;

                using (var fluxoImagemCriada = new FileStream(imagemCriadaUpload, FileMode.Open))
                {

                    var imagemRecebida = new MemoryStream();
                    await fluxoImagemCriada.CopyToAsync(imagemRecebida);

                    var meuFormFile = new FormFile(imagemRecebida, 0, imagemRecebida.Length, "name", nomeImagem.Remove(0, 1));

                    produtoViewModel.ImagemUpload = meuFormFile;


                }
                await produtoController.Create(produtoViewModel);
            }


            if (!OperacaoValida()) return View("Resultado");

            ViewBag.Resultado = "Dados Carregado com sucesso!";


            return View("Resultado");
        }
    }



}








//@using App
//@using App.Models
//@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers


//< form asp - action = "Index" asp - controller = "Home" >
//    < label asp -for= "Email" ></ label >
//    < input asp -for= "Email" placeholder = "informe o seu email" />
//    < input type = "submit" value = "Enviar" />
//</ form >