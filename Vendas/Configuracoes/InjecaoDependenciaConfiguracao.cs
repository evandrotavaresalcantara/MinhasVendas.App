using Vendas.Interfaces.Repositorio;
using Vendas.Interfaces.Servico;
using Vendas.Interfaces;
using Vendas.Repositorio;
using Vendas.Servicos;
using Vendas.Notificaoes;

namespace Vendas.Configuracoes
{
    public static class InjecaoDependenciaConfiguracao
    {
        public static void AdicionarInjecaoDependenciaConfiguracao(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IProdutoServico, ProdutoServico>();
            services.AddScoped<IDetalheDeCompraServico, DetalheDeCompraServico>();
            services.AddScoped<IOrdemDeCompraServico, OrdemDeCompraServico>();
            services.AddScoped<IOrdemDeVendaServico, OrdemDeVendaServico>();
            services.AddScoped<IDetalheDeVendaServico, DetalheDeVendaServico>();
            services.AddScoped<ITransacaoDeEstoqueServico, TransacaoDeEstoqueServico>();
            services.AddScoped<IFornecedorServico, FornecedorServico>();
            services.AddScoped<IClienteServico, ClienteServico>();

            services.AddScoped<IClienteRespositorio, ClienteRepositorio>();
            services.AddScoped<IClienteEnderecoRepositorio, ClienteEnderecoRepositorio>();
            services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
            services.AddScoped<IFornecedorEnderecoRepositorio, FornecedorEnderecoRepositorio>();
            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            services.AddScoped<IProdutoCategoriaRepositorio, ProdutoCategoriaRepositorio>();
            services.AddScoped<IOrdemDeCompraRepositorio, OrdemDeCompraRepositorio>();
            services.AddScoped<IOrdemDeVendaRepositorio, OrdemDeVendaRepositorio>();
            services.AddScoped<IDetalheDeCompraRepositorio, DetalheDeCompraRepositorio>();
            services.AddScoped<IDetalheDeVendaRepositorio, DetalheDeVendaRepositorio>();
            services.AddScoped<ITransacaoDeEstoqueRepositorio, TransacaoDeEstoqueRepositorio>();
            services.AddScoped<IPrecoDeProdutoHistoricoRepositorio, PrecoDeProdutoHistoricoRepositorio>();
        }
    }
}
