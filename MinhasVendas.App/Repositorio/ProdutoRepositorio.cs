using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Repositorio
{
    public class ProdutoRepositorio : Repositorio<Produto>, IProdutoRepositorio
    {
        public ProdutoRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }

        public ListaPaginada<Produto> ObterProdutosPaginacaoLista(ProdutosParametros produtosParametros)
        {
            if (produtosParametros.PesquisaTexto != null)
            {
                produtosParametros.NumeroDePagina = 1;
            }
            else
            {
                produtosParametros.PesquisaTexto = produtosParametros.FiltroAtual;
            }

            IQueryable<Produto> clientes = Obter();

            if (!String.IsNullOrEmpty(produtosParametros.PesquisaTexto))
            {
                clientes = clientes.Where(p => p.Nome.Contains(produtosParametros.PesquisaTexto));
            }


            switch (produtosParametros.OrdemDeClassificacao)
            {
                case "nome_descendente":
                    clientes = clientes.OrderByDescending(o => o.Nome);
                    break;
                case "cidade":
                    clientes = clientes.OrderBy(o => o.Codigo);
                    break;
                case "codigo_descendente":
                    clientes = clientes.OrderByDescending(o => o.Codigo);
                    break;
                default:
                    clientes = clientes.OrderBy(o => o.Nome);
                    break;
            }

            return ListaPaginada<Produto>.ParaListaPaginada(clientes,
                    produtosParametros.NumeroDePagina,
                    produtosParametros.TamanhoDePagina);
        }
    }
}
