using MauiAppMinhasCompras8.Models;

namespace MauiAppMinhasCompras8;

public partial class RelatorioPage : ContentPage
{
    public RelatorioPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var produtos = await App.Db.GetAll();

        var relatorio = produtos
            .GroupBy(p => p.Categoria)
            .Select(g => new CategoriaRelatorio
            {
                Categoria = g.Key,
                Produtos = g.Select(p => new ProdutoRelatorio
                {
                    Descricao = p.Descricao,
                    Quantidade = p.Quantidade,
                    Preco = p.Preco,
                    Total = p.Quantidade * p.Preco
                }).ToList(),

                Total = g.Sum(p => p.Quantidade * p.Preco)
            })
            .OrderBy(x => x.Categoria)
            .ToList();

        RelatorioCollection.ItemsSource = relatorio;
    }
}

public class CategoriaRelatorio
{
    public string Categoria { get; set; }

    public List<ProdutoRelatorio> Produtos { get; set; }

    public double Total { get; set; }
}

public class ProdutoRelatorio
{
    public string Descricao { get; set; }

    public double Quantidade { get; set; }

    public double Preco { get; set; }

    public double Total { get; set; }
}