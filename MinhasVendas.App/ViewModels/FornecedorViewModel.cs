using MinhasVendas.App.Models;

namespace MinhasVendas.App.ViewModels
{
    public class FornecedorViewModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        ICollection<OrdemDeCompraViewModel>? OrdemDeCompras { get; set; }
    }
}
