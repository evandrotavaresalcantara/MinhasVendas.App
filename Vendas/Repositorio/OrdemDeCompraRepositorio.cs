﻿using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;
using Vendas.Paginacao;

namespace Vendas.Repositorio
{
    public class OrdemDeCompraRepositorio : Repositorio<OrdemDeCompra>, IOrdemDeCompraRepositorio
    {
        public OrdemDeCompraRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
        {
        }


        public ListaPaginada<OrdemDeCompra> ObterOrdemDecomprasPaginacaoLista(OrdemDeComprasParametros ordemDeComprasParametros)
        {

            if (ordemDeComprasParametros.PesquisaTexto != null)
            {
                ordemDeComprasParametros.NumeroDePagina = 1;
            }
            else
            {
                ordemDeComprasParametros.PesquisaTexto = ordemDeComprasParametros.FiltroAtual;
            }

            IQueryable<OrdemDeCompra> ordemDeCompras = Obter().Include(f => f.Fornecedor);

            if (!String.IsNullOrEmpty(ordemDeComprasParametros.PesquisaTexto))
            {
                ordemDeCompras = ordemDeCompras.Where(p => p.Fornecedor.Nome.Contains(ordemDeComprasParametros.PesquisaTexto));
            }


            switch (ordemDeComprasParametros.OrdemDeClassificacao)
            {
                case "dataDeCriacao_descendente":
                    ordemDeCompras = ordemDeCompras.OrderByDescending(o => o.DataDeCriacao.ToString());
                    break;
                case "statusOrdemDeCompra":
                    ordemDeCompras = ordemDeCompras.OrderBy(o => o.StatusOrdemDeCompra);
                    break;
                case "statusOrdemDeCompra_descendente":
                    ordemDeCompras = ordemDeCompras.OrderByDescending(o => o.StatusOrdemDeCompra);
                    break;
                default:
                    ordemDeCompras = ordemDeCompras.OrderBy(o => o.DataDeCriacao.ToString());
                    break;
            }

            return ListaPaginada<OrdemDeCompra>.ParaListaPaginada(ordemDeCompras,
                    ordemDeComprasParametros.NumeroDePagina,
                    ordemDeComprasParametros.TamanhoDePagina);
        }
    }
}
