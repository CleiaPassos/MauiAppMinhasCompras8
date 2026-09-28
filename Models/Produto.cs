using SQLite;
namespace MauiAppMinhasCompras8.Models;

public class Produto
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Descricao { get; set; }

    public double Quantidade { get; set; }

    public double Preco { get; set; }

    public string Categoria { get; set; }
}