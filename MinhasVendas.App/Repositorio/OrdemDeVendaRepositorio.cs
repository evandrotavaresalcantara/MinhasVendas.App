using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;
using MinhasVendas.App.Paginacao;

namespace MinhasVendas.App.Repositorio;

public class OrdemDeVendaRepositorio : Repositorio<OrdemDeVenda>, IOrdemDeVendaRepositorio
{
    public OrdemDeVendaRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    {
    }

    public ListaPaginada<OrdemDeVenda> ObterOrdemDeVendasPaginacaoLista(OrdemDeVendasParametros ordemDeVendasParametros)
    {

        if (ordemDeVendasParametros.PesquisaTexto != null)
        {
            ordemDeVendasParametros.NumeroDePagina = 1;
        }
        else
        {
            ordemDeVendasParametros.PesquisaTexto = ordemDeVendasParametros.FiltroAtual;
        }

        IQueryable<OrdemDeVenda> ordemDeVendas = Obter().Include(c => c.Cliente);

        if (!String.IsNullOrEmpty(ordemDeVendasParametros.PesquisaTexto))
        {
            ordemDeVendas = ordemDeVendas.Where(p => p.Cliente.Nome.Contains(ordemDeVendasParametros.PesquisaTexto));
        }


        switch (ordemDeVendasParametros.OrdemDeClassificacao)
        {
            case "dataDeVenda_descendente":
                ordemDeVendas = ordemDeVendas.OrderBy(o => o.DataDeCriacao.ToString());
                break;
            case "statusOrdemDeVenda":
                ordemDeVendas = ordemDeVendas.OrderBy(o => o.StatusOrdemDeVenda);
                break;
            case "statusOrdemDeVenda_descendente":
                ordemDeVendas = ordemDeVendas.OrderByDescending(o => o.StatusOrdemDeVenda);
                break;
            default:
                ordemDeVendas = ordemDeVendas.OrderByDescending(o => o.DataDeCriacao.ToString());
                break;
        }

        return ListaPaginada<OrdemDeVenda>.ParaListaPaginada(ordemDeVendas,
                ordemDeVendasParametros.NumeroDePagina,
                ordemDeVendasParametros.TamanhoDePagina);
    }
}
