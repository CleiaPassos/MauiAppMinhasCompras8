using MauiAppMinhasCompras8.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras8;

public partial class MainPage : ContentPage
{
    // Lista que aparece na tela
    private ObservableCollection<Produto> produtos = new();

    // Lista completa dos produtos
    private List<Produto> todosProdutos = new();

    public MainPage()
    {
        InitializeComponent();

        ProdutosCollectionView.ItemsSource = produtos;

        // Começa mostrando todas as categorias
        FiltroCategoriaPicker.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarProdutosAsync();
    }

    private async Task CarregarProdutosAsync()
    {
        todosProdutos = await App.Db.GetAll();

        AplicarFiltro();
    }

    private async void OnAdicionarProdutoClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DescricaoEntry.Text))
        {
            await DisplayAlert(
                "Atenção",
                "Informe a descrição do produto.",
                "OK");

            return;
        }

        if (!double.TryParse(QuantidadeEntry.Text, out double quantidade))
        {
            await DisplayAlert(
                "Atenção",
                "Informe uma quantidade válida.",
                "OK");

            return;
        }

        if (!double.TryParse(PrecoEntry.Text, out double preco))
        {
            await DisplayAlert(
                "Atenção",
                "Informe um preço válido.",
                "OK");

            return;
        }

        if (CategoriaPicker.SelectedItem == null)
        {
            await DisplayAlert(
                "Atenção",
                "Selecione uma categoria.",
                "OK");

            return;
        }

        string categoria = CategoriaPicker.SelectedItem.ToString();

        var produto = new Produto
        {
            Descricao = DescricaoEntry.Text,
            Quantidade = quantidade,
            Preco = preco,
            Categoria = categoria
        };

        await App.Db.Insert(produto);

        // Adiciona o produto à lista completa
        todosProdutos.Add(produto);

        // Atualiza a lista aplicando os filtros atuais
        AplicarFiltro();

        // Limpa os campos
        DescricaoEntry.Text = string.Empty;
        QuantidadeEntry.Text = string.Empty;
        PrecoEntry.Text = string.Empty;
        CategoriaPicker.SelectedItem = null;
    }

    // Pesquisa pelo nome do produto
    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        AplicarFiltro();
    }

    // Filtro por categoria
    private void OnFiltroCategoriaChanged(object sender, EventArgs e)
    {
        AplicarFiltro();
    }

    // Aplica pesquisa por nome e filtro por categoria
    private void AplicarFiltro()
    {
        string textoBusca = SearchBarProdutos.Text?.Trim() ?? string.Empty;

        string categoriaSelecionada =
            FiltroCategoriaPicker.SelectedItem?.ToString() ?? "Todas";

        var produtosFiltrados = todosProdutos
            .Where(p =>
                (string.IsNullOrWhiteSpace(textoBusca) ||
                 p.Descricao.Contains(
                     textoBusca,
                     StringComparison.OrdinalIgnoreCase))
                &&
                (categoriaSelecionada == "Todas" ||
                 p.Categoria == categoriaSelecionada))
            .ToList();

        produtos.Clear();

        foreach (var produto in produtosFiltrados)
        {
            produtos.Add(produto);
        }
    }

    // Abre o relatório de compras por categoria
    private async void OnRelatorioClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioPage());
    }
}
