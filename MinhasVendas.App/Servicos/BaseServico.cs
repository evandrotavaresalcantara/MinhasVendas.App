﻿using FluentValidation;
using FluentValidation.Results;
using MinhasVendas.App.Interfaces.Servico;
using MinhasVendas.App.Models;
using MinhasVendas.App.Notificador;

namespace MinhasVendas.App.Servicos;

public abstract class BaseServico
{
    private readonly INotificador _notificador;

    public BaseServico(INotificador notificador)
    {
        _notificador = notificador; 
    }

    protected void Notificar(string mensagem)
    {
        _notificador.Handle(new Notificacao(mensagem));
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            Notificar(error.ErrorMessage);
        }
    }

    protected bool ExecutarValidacaoEntidade<TV, TE>(TV validacao, TE entidade) where TV: AbstractValidator<TE> where TE: Entidade
    {
        var validador = validacao.Validate(entidade);

        if (validador.IsValid) return true;

        Notificar(validador);

        return false;
    }
}
