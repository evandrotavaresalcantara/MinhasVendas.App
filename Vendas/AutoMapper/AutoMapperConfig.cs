using AutoMapper;
using Vendas.Models;
using Vendas.ViewModels;

namespace Vendas.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        { 
            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<Produto, ProdutoViewModel>().ReverseMap();  
            CreateMap<OrdemDeCompra,  OrdemDeCompraViewModel>().ReverseMap();
            CreateMap<OrdemDeVenda,  OrdemDeVendaViewModel>().ReverseMap();
            CreateMap<DetalheDeCompra, DetalheDeCompraViewModel>().ReverseMap();
            CreateMap<DetalheDeVenda, DetalheDeVendaViewModel>().ReverseMap();
            CreateMap<ClienteEndereco, ClienteEnderecoViewModel>().ReverseMap();
            CreateMap<FornecedorEndereco, FornecedorEnderecoViewModel>().ReverseMap();
            CreateMap<ProdutoCategoria, ProdutoCategoriaViewModel>().ReverseMap();

        }
    }
}
