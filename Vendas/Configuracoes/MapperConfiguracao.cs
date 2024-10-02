using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Vendas.AutoMapper;

namespace Vendas.Configuracoes
{
    public static class MapperConfiguracao
    {
        public static void AdicionarMapperConfiguracao(this IServiceCollection services)
        {
            var mapeamentoConfiguracao = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });

            IMapper mapper = mapeamentoConfiguracao.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
