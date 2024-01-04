using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Models.Enums;
using MinhasVendas.App.Paginacao;
using MinhasVendas.App.Repositorio;
using Newtonsoft.Json;

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

        public async Task<string> ObterFornecedores(FornecedoresParametros fornecedoresParametros)
        {
            IQueryable<Fornecedor> fornecedoresQuery = _fornecedorRepositorio.Obter().Include(o => o.OrdemDeCompras);

            if (!string.IsNullOrWhiteSpace(fornecedoresParametros.search))
            {
                fornecedoresParametros.search = fornecedoresParametros.search.ToLower();

                fornecedoresQuery = fornecedoresQuery.Where(c =>
                    c.Nome.ToLower().Contains(fornecedoresParametros.search)
                    
                );
            }

            if (Enum.TryParse(fornecedoresParametros.Filtro, out StatusOrdemDeCompra resultado))
            {
                fornecedoresQuery = fornecedoresQuery.Where(c => c.OrdemDeCompras.Any(o => o.StatusOrdemDeCompra == resultado));
            }

            if (fornecedoresParametros.Ordenacao != null)
            {
                var coluna = fornecedoresParametros.Ordenacao;
                var direcao = fornecedoresParametros.Direcao;

                switch (coluna)
                {
                    case 2:
                        fornecedoresQuery = direcao == "asc" ?
                            fornecedoresQuery.OrderBy(c => c.Endereco.Cidade) :
                            fornecedoresQuery.OrderByDescending(c => c.Endereco.Cidade);
                        break;
                }
            }

            var totalRegistros = await fornecedoresQuery.CountAsync();

            var data = await fornecedoresQuery
                .Skip(fornecedoresParametros.start)
                .Take(fornecedoresParametros.lenght)
                .Select(c => new
                {
                    id = c.Id,
                    nome = c.Nome,
                    tipofornecedor = c.TipoFornecedor.ToString(),
                    whatsApp = c.WhatsApp,
                    instagram = c.Instagram,
                    celular = c.Celular,
                    email = c.Email,
                    cidade = c.Endereco.Cidade,

                })
                .ToListAsync();

            string json = JsonConvert.SerializeObject(new
            {
                fornecedoresParametros.draw,
                recordsFiltered = totalRegistros,
                recordsTotal = totalRegistros,
                data
            });

            return json;
        }


    }
}
