using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Models;

namespace MinhasVendas.App.Repositorio;

public class ClienteEnderecoRepositorio : Repositorio<ClienteEndereco>, IClienteEnderecoRepositorio
{
    public ClienteEnderecoRepositorio(MinhasVendasAppContext minhasVendasAppContext) : base(minhasVendasAppContext)
    { }

 }
