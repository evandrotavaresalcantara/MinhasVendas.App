using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Vendas.Configuracoes
{
    public static class GlobalizacaoConfiguracao
    {
        public static void AdicionarGlobalizacaoConfiguracao(this IApplicationBuilder app)
        {
            var defaultCulture = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

          app.UseRequestLocalization(localizationOptions);
        }
    }
}
