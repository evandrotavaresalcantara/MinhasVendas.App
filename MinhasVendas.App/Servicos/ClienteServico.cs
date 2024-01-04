using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Models.Validacoes;
using MinhasVendas.App.Paginacao;
using Newtonsoft.Json;

namespace MinhasVendas.App.Servicos
{
    public class ClienteServico : BaseServico, IClienteServico
    {
        private readonly IClienteRespositorio _clienteRespositorio;
        public ClienteServico(INotificador notificador,
                              IClienteRespositorio clienteRespositorio) : base(notificador)
        {
            _clienteRespositorio = clienteRespositorio;
        }

        public async Task Adicionar(Cliente cliente)
        {
            if (!ExecutarValidacaoEntidade(new ClienteValidacao(), cliente))
            {
                return;
            }
            await _clienteRespositorio.Adicionar(cliente);
        }

        public async Task Atualizar(Cliente cliente)
        {
            await _clienteRespositorio.Atualizar(cliente);
        }


        public async Task Remover(int id)
        {
            if (_clienteRespositorio.ObterClienteProdutoEndereco(id).Result.OrdemDeVendas.Any())
            {
                Notificar("O cliente possui ordem de vendas cadastradas!");
                return;
            }
            await _clienteRespositorio.Remover(id);
        }

        public async Task<string> ObterClientes(ClientesParametros clientesParametros)
        {
            IQueryable<Cliente> clientesQuery = _clienteRespositorio.Obter().Include(o => o.OrdemDeVendas);

            if (!string.IsNullOrWhiteSpace(clientesParametros.search))
            {
                clientesParametros.search = clientesParametros.search.ToLower();

                clientesQuery = clientesQuery.Where(c =>
                    c.Nome.ToLower().Contains(clientesParametros.search) || 
                    c.SobreNome.ToLower().Contains(clientesParametros.search)
                );
            }


            if (Enum.TryParse(clientesParametros.Filtro, out StatusOrdemDeVenda resultado))
            {
                clientesQuery = clientesQuery.Where(c => c.OrdemDeVendas.Any(o => o.StatusOrdemDeVenda == resultado));
            }

            if (clientesParametros.Ordenacao != null)
            {
                var coluna = clientesParametros.Ordenacao;
                var direcao = clientesParametros.Direcao;

                switch (coluna)
                {
                    case 3:
                        clientesQuery = direcao == "asc" ?
                            clientesQuery.OrderBy(c => c.Endereco.Cidade) :
                            clientesQuery.OrderByDescending(c => c.Endereco.Cidade);
                        break;
                }
            }

            var totalRegistros = await clientesQuery.CountAsync();

            var data = await clientesQuery
                .Skip(clientesParametros.start)
                .Take(clientesParametros.lenght)
                .Select(c => new
                {
                    id = c.Id,
                    nome = c.Nome,
                    sobrenome = c.SobreNome,
                    whatsApp = c.WhatsApp,
                    celular = c.Celular,
                    instagram = c.Instagram,
                    email = c.Email,
                    cidade = c.Endereco.Cidade,
                    
                })
                .ToListAsync();

            string json = JsonConvert.SerializeObject(new 
            {
                clientesParametros.draw,
                recordsFiltered = totalRegistros,
                recordsTotal = totalRegistros,
                data
            });

            return json;

        }

    }
}
