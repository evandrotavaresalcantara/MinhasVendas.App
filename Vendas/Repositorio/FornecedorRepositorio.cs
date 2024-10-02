using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Repositorio
{
    public class FornecedorRepositorio : Repositorio<Fornecedor>, IFornecedorRepositorio
    {
        public FornecedorRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
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

        public ListaPaginada<Fornecedor> ObterFornecedoresaginacaoLista(FornecedoresParametros fornecedoresParametros)
        {
            if (fornecedoresParametros.PesquisaTexto != null)
            {
                fornecedoresParametros.NumeroDePagina = 1;
            }
            else
            {
                fornecedoresParametros.PesquisaTexto = fornecedoresParametros.FiltroAtual;
            }

            IQueryable<Fornecedor> clientes = Obter().Include(c => c.Endereco);

            if (!String.IsNullOrEmpty(fornecedoresParametros.PesquisaTexto))
            {
                clientes = clientes.Where(p => p.Nome.Contains(fornecedoresParametros.PesquisaTexto));
            }


            switch (fornecedoresParametros.OrdemDeClassificacao)
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

            return ListaPaginada<Fornecedor>.ParaListaPaginada(clientes,
                    fornecedoresParametros.NumeroDePagina,
                    fornecedoresParametros.TamanhoDePagina);
        }
    }
}
