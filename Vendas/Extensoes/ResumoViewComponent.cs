using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Vendas.Interfaces.Servico;

namespace Vendas.Extensoes
{
    public class ResumoViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public ResumoViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            notificacoes.ForEach(c=> ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));

            return View();
        }
    }
}