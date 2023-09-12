using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio;

public class FornecedorEnderecoRepositorio : Repositorio<FornecedorEndereco>, IFornecedorEnderecoRepositorio
{
    public FornecedorEnderecoRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
