using Microsoft.AspNetCore.Identity;
using Vendas.Data;
using Microsoft.EntityFrameworkCore;

namespace Vendas.Configuracoes
{
    public static class IdentityConfiguracao
    {
        public static void AdicionarIdentityConfiguracao(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Postgres") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            services.AddDbContext<VendasAppContext>(options =>
                options.UseNpgsql(connectionString));
            
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<VendasAppContext>();
            services.AddControllersWithViews();
        }
    }
}
