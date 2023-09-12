using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class FornecedorRepositorio : Repositorio<Fornecedor>, IFornecedorRepositorio
    {
        public FornecedorRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {  }

        public async Task<Fornecedor> ObterFornecedorEndereco(int id)
        {
            return await
                _dbSet.AsNoTracking()
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);

        }

        public async Task<Fornecedor> ObterFornecedorProdutoEndereco(int id)
        {
            return await
                _dbSet.AsNoTracking()
                .Include(e => e.Endereco)
                .Include(o => o.OrdemDeCompras)
                    .ThenInclude(d => d.DetalheDeCompras)
                        .ThenInclude(p => p.Produto)
                            .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
