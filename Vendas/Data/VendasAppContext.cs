using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vendas.Models;

namespace Vendas.Data
{
    public class VendasAppContext : IdentityDbContext
    {
        public VendasAppContext(DbContextOptions<VendasAppContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente>? Clientes { get; set; }
        public DbSet<DetalheDeCompra>? DetalheDeCompras { get; set; }
        public DbSet<DetalheDeVenda>? DetalheDeVendas { get; set; }
        public DbSet<Fornecedor>? Fornecedores { get; set; }
        public DbSet<OrdemDeCompra>? OrdemDeCompras { get; set; }
        public DbSet<OrdemDeVenda>? OrdemDeVendas { get; set; }
        public DbSet<Produto>? Produtos { get; set; }
        public DbSet<ProdutoCategoria> ProdutoCategorias { get; set; }
        public DbSet<TransacaoDeEstoque>? TransacaoDeEstoques { get; set; }
        public DbSet<ClienteEndereco> ClienteEndereco { get; set; } = default!;
        public DbSet<FornecedorEndereco> FornecedorEndereco { get; set; } = default!;
        public DbSet<PrecoDeProdutoHistorico> PrecoDeProdutoHistoricos { get; set; } = default!;


    }
}
