using Microsoft.EntityFrameworkCore;
using Vendas.Data;
using Vendas.Interfaces.Repositorio;
using Vendas.Models;

namespace Vendas.Repositorio;

public class ClienteEnderecoRepositorio : Repositorio<ClienteEndereco>, IClienteEnderecoRepositorio
{
    public ClienteEnderecoRepositorio(VendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
