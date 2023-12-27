using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio;

public class PrecoDeProdutoHistoricoRepositorio : Repositorio<PrecoDeProdutoHistorico>, IPrecoDeProdutoHistoricoRepositorio
{
    public PrecoDeProdutoHistoricoRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
