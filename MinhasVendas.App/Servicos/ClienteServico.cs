using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Validacoes;

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
    }
}
