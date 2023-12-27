﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhasVendas.App.Data;

#nullable disable

namespace MinhasVendas.App.Migrations
{
    [DbContext(typeof(MinhasVendasAppContext))]
    [Migration("20231019112741_historicodepreco")]
    partial class historicodepreco
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Celular")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Documento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Instagram")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SobreNome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WhatsApp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.ClienteEndereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complemento")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId")
                        .IsUnique();

                    b.ToTable("ClienteEndereco");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeCompra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("CustoUnitario")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataDeRecebimento")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrdemDeCompraId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RegistradoTransacaoDeEstoque")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TransacaoDeEstoqueId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrdemDeCompraId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("DetalheDeCompras");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Desconto")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrdemDeVendaId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("PrecoUnitario")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RegistroTransacaoDeEstoque")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TransacaoDeEstoqueId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrdemDeVendaId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("DetalheDeVendas");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Fornecedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Celular")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Documento")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Instagram")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TipoFornecedor")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WhatsApp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Fornecedores");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.FornecedorEndereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Complemento")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FornecedorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FornecedorId")
                        .IsUnique();

                    b.ToTable("FornecedorEndereco");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeCompra", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataDeCriacao")
                        .HasColumnType("TEXT");

                    b.Property<int>("FormaDePagamento")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FornecedorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StatusOrdemDeCompra")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ValorDeFrete")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FornecedorId");

                    b.ToTable("OrdemDeCompras");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeVenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClienteId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataDeCriacao")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DataDePagamento")
                        .HasColumnType("TEXT");

                    b.Property<int>("FormaDePagamento")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StatusOrdemDeVenda")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ValorDeFrete")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("OrdemDeVendas");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.PrecoDeProdutoHistorico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataAtualizacao")
                        .HasColumnType("TEXT");

                    b.Property<int>("EstoqueAtual")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("MarkUp")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoDeCusto")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoDeVenda")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoId");

                    b.ToTable("PrecoDeProdutoHistoricos");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Produto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Ativo")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DataDeCadastro")
                        .HasColumnType("TEXT");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EstoqueAtual")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Imagem")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("MarkUp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoDeCusto")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrecoDeVenda")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProdutoCategoriaId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProdutoCategoriaId");

                    b.ToTable("Produtos");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.ProdutoCategoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProdutoCategorias");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.TransacaoDeEstoque", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DataDeTransacao")
                        .HasColumnType("TEXT");

                    b.Property<int?>("OrdemDeCompraId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrdemDeVendaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProdutoId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantidade")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TipoDransacaoDeEstoque")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrdemDeCompraId");

                    b.HasIndex("OrdemDeVendaId");

                    b.HasIndex("ProdutoId");

                    b.ToTable("TransacaoDeEstoques");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MinhasVendas.App.Models.ClienteEndereco", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Cliente", "Cliente")
                        .WithOne("Endereco")
                        .HasForeignKey("MinhasVendas.App.Models.ClienteEndereco", "ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeCompra", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.OrdemDeCompra", "OrdemDeCompra")
                        .WithMany("DetalheDeCompras")
                        .HasForeignKey("OrdemDeCompraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany("DetalheDeCompras")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdemDeCompra");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.DetalheDeVenda", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.OrdemDeVenda", "OrdemDeVenda")
                        .WithMany("DetalheDeVendas")
                        .HasForeignKey("OrdemDeVendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany("DetalheDeVendas")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdemDeVenda");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.FornecedorEndereco", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Fornecedor", "Fornecedor")
                        .WithOne("Endereco")
                        .HasForeignKey("MinhasVendas.App.Models.FornecedorEndereco", "FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeCompra", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Fornecedor", "Fornecedor")
                        .WithMany("OrdemDeCompras")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fornecedor");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeVenda", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Cliente", "Cliente")
                        .WithMany("OrdemDeVendas")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.PrecoDeProdutoHistorico", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany("PrecoDeProdutoHistoricos")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Produto", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.ProdutoCategoria", "ProdutoCategoria")
                        .WithMany("Produtos")
                        .HasForeignKey("ProdutoCategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProdutoCategoria");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.TransacaoDeEstoque", b =>
                {
                    b.HasOne("MinhasVendas.App.Models.OrdemDeCompra", "OrdemDeCompra")
                        .WithMany()
                        .HasForeignKey("OrdemDeCompraId");

                    b.HasOne("MinhasVendas.App.Models.OrdemDeVenda", "OrdemDeVenda")
                        .WithMany("TransacaoDeEstoques")
                        .HasForeignKey("OrdemDeVendaId");

                    b.HasOne("MinhasVendas.App.Models.Produto", "Produto")
                        .WithMany("TransacaoDeEstoques")
                        .HasForeignKey("ProdutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrdemDeCompra");

                    b.Navigation("OrdemDeVenda");

                    b.Navigation("Produto");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Cliente", b =>
                {
                    b.Navigation("Endereco");

                    b.Navigation("OrdemDeVendas");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Fornecedor", b =>
                {
                    b.Navigation("Endereco");

                    b.Navigation("OrdemDeCompras");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeCompra", b =>
                {
                    b.Navigation("DetalheDeCompras");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.OrdemDeVenda", b =>
                {
                    b.Navigation("DetalheDeVendas");

                    b.Navigation("TransacaoDeEstoques");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.Produto", b =>
                {
                    b.Navigation("DetalheDeCompras");

                    b.Navigation("DetalheDeVendas");

                    b.Navigation("PrecoDeProdutoHistoricos");

                    b.Navigation("TransacaoDeEstoques");
                });

            modelBuilder.Entity("MinhasVendas.App.Models.ProdutoCategoria", b =>
                {
                    b.Navigation("Produtos");
                });
#pragma warning restore 612, 618
        }
    }
}