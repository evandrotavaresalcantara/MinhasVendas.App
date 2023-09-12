using FluentValidation;

namespace MinhasVendas.App.Models.Validacoes;

public class ClienteValidacao : AbstractValidator<Cliente>
{
    public ClienteValidacao()
    {
        RuleFor(c => c.Nome)
                    .NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
                    .Length(2, 100)
                    .WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}






//    RuleFor(f => f.Nome)
//                .NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
//                .Length(2, 100)
//                .WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

//    When(f => f.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
//            {
//        RuleFor(f => f.Documento.Length).Equal(CpfValidacao.TamanhoCpf)
//            .WithMessage("FLUENTVALIDATION - O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
//        RuleFor(f => CpfValidacao.Validar(f.Documento)).Equal(true)
//            .WithMessage("FLUENTVALIDATION - O documento fornecido é inválido.");
//    });

//            When(f => f.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
//            {
//        RuleFor(f => f.Documento.Length).Equal(CnpjValidacao.TamanhoCnpj)
//            .WithMessage("FLUENTVALIDATION - O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
//        RuleFor(f => CnpjValidacao.Validar(f.Documento)).Equal(true)
//            .WithMessage("FLUENTVALIDATION - O documento fornecido é inválido.");
//    });
//}


// RuleFor(c => c.Logradouro)
//.NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
//.Length(2, 200).WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

// RuleFor(c => c.Bairro)
//     .NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
//     .Length(2, 100).WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} EnderecoValidation");

// RuleFor(c => c.Cep)
//     .NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
//     .Length(8).WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter {MaxLength} caracteres");

// RuleFor(c => c.Cidade)
//     .NotEmpty().WithMessage("FLUENTVALIDATION - A campo {PropertyName} precisa ser fornecida")
//     .Length(2, 100).WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

// RuleFor(c => c.Estado)
//     .NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
//     .Length(2, 50).WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

// RuleFor(c => c.Numero)
//     .NotEmpty().WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ser fornecido")
//     .Length(1, 50).WithMessage("FLUENTVALIDATION - O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
