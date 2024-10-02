using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Repositorio
{
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRespositorio
    {
        public ClienteRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
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

        public ListaPaginada<Cliente> ObterClientesPaginacaoLista(ClientesParametros clientesParametros)
        {

            if (clientesParametros.PesquisaTexto != null)
            {
                clientesParametros.NumeroDePagina = 1;
            }
            else
            {
                clientesParametros.PesquisaTexto = clientesParametros.FiltroAtual;
            }

            IQueryable<Cliente> clientes = Obter().Include(c => c.Endereco);

            if (!String.IsNullOrEmpty(clientesParametros.PesquisaTexto))
            {
                clientes = clientes.Where(p => p.Nome.Contains(clientesParametros.PesquisaTexto));
            }


            switch (clientesParametros.OrdemDeClassificacao)
            {
                case "nome_descendente":
                    clientes = clientes.OrderByDescending(o => o.Nome);
                    break;
                case "cidade":
                    clientes = clientes.OrderBy(o => o.Endereco.Cidade);
                    break;
                case "cidade_descendente":
                    clientes = clientes.OrderByDescending(o => o.Endereco.Cidade);
                    break;
                default:
                    clientes = clientes.OrderBy(o => o.Nome);
                    break;
            }

            return ListaPaginada<Cliente>.ParaListaPaginada(clientes,
                    clientesParametros.NumeroDePagina,
                    clientesParametros.TamanhoDePagina);
        }
    }
}
