using SQLite;
using MauiAppMinhasCompras8.Models;

namespace MauiAppMinhasCompras8.Data;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        string caminho = Path.Combine(
            FileSystem.AppDataDirectory,
            "compras.db3");

        _database = new SQLiteAsyncConnection(caminho);
    }

    public async Task InicializarBancoAsync()
    {
        await _database.CreateTableAsync<Produto>();
    }

    public async Task<List<Produto>> ListarProdutosAsync()
    {
        return await _database.Table<Produto>().ToListAsync();
    }

    public async Task<int> AdicionarProdutoAsync(Produto produto)
    {
        return await _database.InsertAsync(produto);
    }

    public async Task<int> ExcluirProdutoAsync(Produto produto)
    {
        return await _database.DeleteAsync(produto);
    }
}