using MinhasVendas.App.Models;
using System.ComponentModel.DataAnnotations;

namespace MinhasVendas.App.ViewModels
{
    public class ClienteViewModel
    {
        [Key]
        public int Id { get; set; }

        public string? Nome { get; set; }

        ICollection<OrdemDeVendaViewModel>? OrdemDeVendas { get; set; }
    }
}
