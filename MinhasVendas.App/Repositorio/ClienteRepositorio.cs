using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio
{
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRespositorio
    {
        public ClienteRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        { }

        public async Task<Cliente> ObterClienteEndereco(int id)
        {
            return await
                _dbSet.AsNoTracking()
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);

        }

        public async Task<Cliente> ObterClienteProdutoEndereco(int id)
        {
            return await
                _dbSet.AsNoTracking()
                .Include(e => e.Endereco)
                .Include(o => o.OrdemDeVendas)
                    .ThenInclude(d => d.DetalheDeVendas)
                        .ThenInclude(p => p.Produto)
                            .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
