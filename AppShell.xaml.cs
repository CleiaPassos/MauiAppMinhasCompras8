namespace MauiAppMinhasCompras8;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Items.Add(new ShellContent
        {
            Title = "Home",
            Content = new MainPage(),
            Route = "MainPage"
        });

        Items.Add(new ShellContent
        {
            Title = "Relatório",
            Content = new RelatorioPage(),
            Route = "RelatorioPage"
        });
    }
}