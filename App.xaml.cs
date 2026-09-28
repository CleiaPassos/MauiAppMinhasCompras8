using MauiAppMinhasCompras8.Data;
using System.Globalization;

namespace MauiAppMinhasCompras8;

public partial class App : Application
{
    static SQLiteDatabaseHelper _db;

    public static SQLiteDatabaseHelper Db
    {
        get
        {
            if (_db == null)
            {
                string caminho = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "banco_sqlite_compras.db3");

                _db = new SQLiteDatabaseHelper(caminho);
            }

            return _db;
        }
    }

    public App()
    {
        InitializeComponent();

        Thread.CurrentThread.CurrentCulture =
            new CultureInfo("pt-BR");

        MainPage = new AppShell();
    }
}
