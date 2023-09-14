using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio;

public class ProdutoCategoriaRepositorio : Repositorio<ProdutoCategoria>, IProdutoCategoriaRepositorio
{
    public ProdutoCategoriaRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
