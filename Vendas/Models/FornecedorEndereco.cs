﻿using NuGet.Protocol.Core.Types;

namespace Vendas.Models
{
    public class FornecedorEndereco : Entidade
    {
        public int FornecedorId { get; set; }

        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        public Fornecedor? Fornecedor { get; set; }
    }
}
