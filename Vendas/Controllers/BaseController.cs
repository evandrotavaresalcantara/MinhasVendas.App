using Microsoft.AspNetCore.Mvc;
using Vendas.Interfaces.Servico;

namespace Vendas.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificador _notificador;

        public BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }
        protected void Notificar(string mensagem)
        {
            _notificador.Handle(new Notificaoes.Notificacao(mensagem));
        }
    }
}
