using MauiAppMinhasCompras8.Models;
using SQLite;

namespace MauiAppMinhasCompras8.Data;

public class SQLiteDatabaseHelper
{
    readonly SQLiteAsyncConnection _conn;

    public SQLiteDatabaseHelper(string caminho)
    {
        _conn = new SQLiteAsyncConnection(caminho);

        // Cria a tabela caso ela ainda não exista
        _conn.CreateTableAsync<Produto>().Wait();

        // Verifica se a coluna Categoria já existe
        var colunas = _conn
            .QueryAsync<ColunaTabela>("PRAGMA table_info(Produto)")
            .Result;

        bool categoriaExiste = colunas.Any(c =>
            c.name.Equals("Categoria", StringComparison.OrdinalIgnoreCase));

        // Se a tabela antiga não tiver Categoria, adiciona a coluna
        if (!categoriaExiste)
        {
            _conn.ExecuteAsync(
                "ALTER TABLE Produto ADD COLUMN Categoria TEXT"
            ).Wait();
        }
    }

    public Task<int> Insert(Produto produto)
    {
        return _conn.InsertAsync(produto);
    }

    public Task<List<Produto>> GetAll()
    {
        return _conn.Table<Produto>().ToListAsync();
    }

    public Task<int> Delete(int id)
    {
        return _conn.Table<Produto>()
                    .DeleteAsync(p => p.Id == id);
    }

    public Task<List<Produto>> Search(string texto)
    {
        string sql = "SELECT * FROM Produto WHERE Descricao LIKE ?";

        return _conn.QueryAsync<Produto>(
            sql,
            "%" + texto + "%"
        );
    }

    // Classe usada somente para verificar as colunas da tabela
    private class ColunaTabela
    {
        public string name { get; set; }
    }
}