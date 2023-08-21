namespace MinhasVendas.App.Paginacao;

public class ListaPaginada<T> : List<T>
{
    public int PaginaAtual { get; private set; }
    public int TotalDePaginas { get; private set; }
    public int TamanhoDaPagina { get; private set; }
    public int TotalDeItens { get; private set; }

    public bool TemAnterior => PaginaAtual > 1;
    public bool TemProxima => PaginaAtual < TotalDePaginas;

    public ListaPaginada(List<T> itens, int quantidade, int numeroDaPagina, int tamanhoDaPagina)
    {
        PaginaAtual = numeroDaPagina;
        TotalDePaginas = (int)Math.Ceiling(quantidade / (double)tamanhoDaPagina);
        TamanhoDaPagina = tamanhoDaPagina;
        TotalDeItens = quantidade;

        AddRange(itens);
    }
    public static ListaPaginada<T> ParaListaPaginada(IQueryable<T> fonte, int numeroDaPagina, int tamanhoDaPagina)
    {
        var quantidade = fonte.Count();
        var itens = fonte.Skip((numeroDaPagina - 1) * tamanhoDaPagina).Take(tamanhoDaPagina).ToList();

        return new ListaPaginada<T>(itens, quantidade, numeroDaPagina, tamanhoDaPagina);
    }
}
