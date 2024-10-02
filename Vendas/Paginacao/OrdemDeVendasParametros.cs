using Vendas.Models.Enums;

namespace Vendas.Paginacao;

public class OrdemDeVendasParametros : QueryStringParametros
{
    public string OrdemDeClassificacao { get; set; } = string.Empty;
    public string PesquisaTexto { get; set; } = string.Empty;
    public string FiltroAtual { get; set; } = string.Empty;
    public string Filtro { get; set; } = string.Empty;


    public string search { get; set; } = string.Empty;
    public int start { get; set; }
    public int lenght { get; set; }  
    public int draw { get; set; }
    public int Ordenacao { get; set; }
    public string Direcao { get; set; } = string.Empty;
}
