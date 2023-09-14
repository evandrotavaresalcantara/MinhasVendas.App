namespace MinhasVendas.App.ViewModels
{
    public class ProdutoCategoriaViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public ICollection<ProdutoViewModel>? Produtos { get; set; }
    }
}
