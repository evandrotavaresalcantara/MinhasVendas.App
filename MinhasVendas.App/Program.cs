using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MinhasVendas.App.AutoMapper;
using MinhasVendas.App.Data;
using MinhasVendas.App.Interfaces;
using MinhasVendas.App.Interfaces.Repositorio;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Notificador;
using MinhasVendas.App.Repositorio;
using MinhasVendas.App.Servicos;
using System;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<MinhasVendasAppContext>(options =>
///    options.UseSqlServer(builder.Configuration.GetConnectionString("MinhasVendasAppContext") ?? throw new InvalidOperationException("Connection string 'MinhasVendasAppContext' not found.")));

builder.Services.AddDbContext<MinhasVendasAppContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("MinhaConexao")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MinhasVendasAppContext>();





// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<INotificador, Notificador>();

builder.Services.AddScoped<IProdutoServico, ProdutoServico>();
builder.Services.AddScoped<IDetalheDeCompraServico, DetalheDeCompraServico>();
builder.Services.AddScoped<IOrdemDeCompraServico, OrdemDeCompraServico>();
builder.Services.AddScoped<IOrdemDeVendaServico, OrdemDeVendaServico>();
builder.Services.AddScoped<IDetalheDeVendaServico, DetalheDeVendaServico>();
builder.Services.AddScoped<ITransacaoDeEstoqueServico, TransacaoDeEstoqueServico>();
builder.Services.AddScoped<IFornecedorServico, FornecedorServico>();
builder.Services.AddScoped<IClienteServico, ClienteServico>();


builder.Services.AddScoped<IClienteRespositorio, ClienteRepositorio>();
builder.Services.AddScoped<IClienteEnderecoRepositorio, ClienteEnderecoRepositorio>();
builder.Services.AddScoped<IFornecedorRepositorio, FornecedorRepositorio>();
builder.Services.AddScoped<IFornecedorEnderecoRepositorio, FornecedorEnderecoRepositorio>();
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IProdutoCategoriaRepositorio, ProdutoCategoriaRepositorio>();
builder.Services.AddScoped<IOrdemDeCompraRepositorio, OrdemDeCompraRepositorio>();
builder.Services.AddScoped<IOrdemDeVendaRepositorio, OrdemDeVendaRepositorio>();
builder.Services.AddScoped<IDetalheDeCompraRepositorio, DetalheDeCompraRepositorio>();
builder.Services.AddScoped<IDetalheDeVendaRepositorio, DetalheDeVendaRepositorio>();
builder.Services.AddScoped<ITransacaoDeEstoqueRepositorio, TransacaoDeEstoqueRepositorio>();
builder.Services.AddScoped<IPrecoDeProdutoHistoricoRepositorio, PrecoDeProdutoHistoricoRepositorio>();



var mapeamentoConfiguracao = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperConfig());
});
IMapper mapper = mapeamentoConfiguracao.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();





// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};
app.UseRequestLocalization(localizationOptions);



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
