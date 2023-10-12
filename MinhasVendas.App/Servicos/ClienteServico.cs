using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
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

        public async Task<string> ObterClientes(OrdemDeVendasParametros ordemDeVendasParametros)
        {
            var clientesQuery = _clienteRespositorio.Obter();

            if (!string.IsNullOrWhiteSpace(ordemDeVendasParametros.search))
            {
                ordemDeVendasParametros.search = ordemDeVendasParametros.search.ToLower();

                clientesQuery = clientesQuery.Where(c =>
                    c.Nome.ToLower().Contains(ordemDeVendasParametros.search) || 
                    c.SobreNome.ToLower().Contains(ordemDeVendasParametros.search)
                );
            }

            var totalRegistros = await clientesQuery.CountAsync();

            var data = await clientesQuery
                .Skip(ordemDeVendasParametros.start)
                .Take(ordemDeVendasParametros.lenght)
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
                ordemDeVendasParametros.draw,
                recordsFiltered = totalRegistros,
                recordsTotal = totalRegistros,
                data
            });


            return json;




        }


    }
}
