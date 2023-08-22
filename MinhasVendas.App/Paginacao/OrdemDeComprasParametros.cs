﻿namespace MinhasVendas.App.Paginacao;

public class OrdemDeComprasParametros : QueryStringParametros
{
    public string OrdemDeClassificacao { get; set; } = string.Empty;
    public string PesquisaTexto { get; set; } = string.Empty;
    public string FiltroAtual { get; set; } = string.Empty;
}
