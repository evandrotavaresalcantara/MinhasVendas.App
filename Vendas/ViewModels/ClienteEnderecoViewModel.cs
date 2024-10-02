﻿using Vendas.Models;

namespace Vendas.ViewModels
{
    public class ClienteEnderecoViewModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }

        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        public ClienteViewModel? Cliente { get; set; }
    }
}
