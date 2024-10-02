using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio;

public class ProdutoCategoriaRepositorio : Repositorio<ProdutoCategoria>, IProdutoCategoriaRepositorio
{
    public ProdutoCategoriaRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
