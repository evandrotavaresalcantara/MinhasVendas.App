namespace Vendas.Models;

public class ProdutoCategoria : Entidade
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;

    public ICollection<Produto>? Produtos { get; set; }

}
