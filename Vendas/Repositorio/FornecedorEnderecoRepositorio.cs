using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio;

public class FornecedorEnderecoRepositorio : Repositorio<FornecedorEndereco>, IFornecedorEnderecoRepositorio
{
    public FornecedorEnderecoRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
