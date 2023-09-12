using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Repositorio;

namespace MinhasVendas.App.Servicos
{
    public class FornecedorServico : BaseServico, IFornecedorServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        public FornecedorServico(INotificador notificador,
                                 IFornecedorRepositorio fornecedorRepositorio) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            await _fornecedorRepositorio.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            await _fornecedorRepositorio.Atualizar(fornecedor);
        }

        public async Task Remover(int id)
        {
            if (_fornecedorRepositorio.ObterFornecedorProdutoEndereco(id).Result.OrdemDeCompras.Any())
            {
                Notificar("O fornecedor possui ordem de compra cadastrada!");
                return;
            }
            await _fornecedorRepositorio.Remover(id);
        }
    }
}
