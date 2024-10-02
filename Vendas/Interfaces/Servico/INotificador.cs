using Microsoft.AspNetCore.Components.Web;
using Vendas.Notificaoes;

namespace Vendas.Interfaces.Servico
{
    public interface INotificador
    {
        void Handle(Notificacao notificacao);
        List<Notificacao> ObterNotificacoes();
        bool TemNotificacao();
    }
}
