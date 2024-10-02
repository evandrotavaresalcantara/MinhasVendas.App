using Vendas.Configuracoes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AdicionarIdentityConfiguracao(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AdicionarMvconfiguracao();
builder.Services.AdicionarInjecaoDependenciaConfiguracao();
builder.Services.AdicionarMapperConfiguracao();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.AdicionarGlobalizacaoConfiguracao();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
