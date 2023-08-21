namespace MinhasVendas.App.Paginacao;

public abstract class QueryStringParametros
{

    const int tamanhoDePaginaMaximo = 50;

    public int NumeroDePagina { get; set; } = 1;
    private int _tamanhoDePagina = 10;
    public int TamanhoDePagina
    {
        get
        {
            return _tamanhoDePagina;
        }
        set
        {
            _tamanhoDePagina = (value > tamanhoDePaginaMaximo) ? tamanhoDePaginaMaximo : value;
        }
    }
}