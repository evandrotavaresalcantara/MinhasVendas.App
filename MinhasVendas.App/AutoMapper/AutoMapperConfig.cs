using AutoMapper;
using MinhasVendas.App.Models;
using MinhasVendas.App.ViewModels;

namespace MinhasVendas.App.AutoMapper
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
        }
    }
}
