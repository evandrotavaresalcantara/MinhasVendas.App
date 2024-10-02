using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio;

public class PrecoDeProdutoHistoricoRepositorio : Repositorio<PrecoDeProdutoHistorico>, IPrecoDeProdutoHistoricoRepositorio
{
    public PrecoDeProdutoHistoricoRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
