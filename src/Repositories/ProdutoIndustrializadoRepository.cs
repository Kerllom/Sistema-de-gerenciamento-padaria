using MySql.Data.MySqlClient;
using PadariaApp.Data;
using PadariaApp.Models;

namespace PadariaApp.Repositories;

// CRUD de ProdutoIndustrializado.
// Segue EXATAMENTE o mesmo padrao do ProdutoPerecivelRepository -
// duas tabelas (produto + produto_industrializado), mesmo id.
public class ProdutoIndustrializadoRepository : BaseRepository
{
    // ----------------------------- CREATE -----------------------------
    public int Inserir(ProdutoIndustrializado produto)
    {
        var sqlBase = @"INSERT INTO produto (nome, preco, quantidade_estoque, tipo)
                        VALUES (@nome, @preco, @qtd, 'INDUSTRIALIZADO');";

        var idGerado = ExecuteInsertAndGetId(sqlBase,
            new MySqlParameter("@nome", produto.Nome),
            new MySqlParameter("@preco", produto.Preco),
            new MySqlParameter("@qtd", produto.QuantidadeEstoque));

        var sqlFilha = @"INSERT INTO produto_industrializado (id, marca, codigo_barras)
                         VALUES (@id, @marca, @codigo);";

        // (object?)... evita erro quando CodigoBarras for null.
        // Convertemos null em DBNull.Value, que e como o MySQL representa.
        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@id", idGerado),
            new MySqlParameter("@marca", produto.Marca),
            new MySqlParameter("@codigo", (object?)produto.CodigoBarras ?? DBNull.Value));

        produto.Id = idGerado;
        return idGerado;
    }

    // ------------------------------ READ ------------------------------
    public List<ProdutoIndustrializado> ListarTodos()
    {
        var sql = @"SELECT p.id, p.nome, p.preco, p.quantidade_estoque,
                           pi.marca, pi.codigo_barras
                    FROM produto p
                    INNER JOIN produto_industrializado pi ON p.id = pi.id;";

        var lista = new List<ProdutoIndustrializado>();

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        using var leitor = comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(new ProdutoIndustrializado
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nome"),
                Preco = leitor.GetDecimal("preco"),
                QuantidadeEstoque = leitor.GetInt32("quantidade_estoque"),
                Marca = leitor.GetString("marca"),
                // IsDBNull verifica se a coluna veio nula do banco.
                CodigoBarras = leitor.IsDBNull(leitor.GetOrdinal("codigo_barras"))
                                ? null
                                : leitor.GetString("codigo_barras")
            });
        }

        return lista;
    }

    public ProdutoIndustrializado? BuscarPorId(int id)
    {
        var sql = @"SELECT p.id, p.nome, p.preco, p.quantidade_estoque,
                           pi.marca, pi.codigo_barras
                    FROM produto p
                    INNER JOIN produto_industrializado pi ON p.id = pi.id
                    WHERE p.id = @id;";

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@id", id);

        using var leitor = comando.ExecuteReader();
        if (!leitor.Read()) return null;

        return new ProdutoIndustrializado
        {
            Id = leitor.GetInt32("id"),
            Nome = leitor.GetString("nome"),
            Preco = leitor.GetDecimal("preco"),
            QuantidadeEstoque = leitor.GetInt32("quantidade_estoque"),
            Marca = leitor.GetString("marca"),
            CodigoBarras = leitor.IsDBNull(leitor.GetOrdinal("codigo_barras"))
                            ? null
                            : leitor.GetString("codigo_barras")
        };
    }

    // ----------------------------- UPDATE -----------------------------
    public void Atualizar(ProdutoIndustrializado produto)
    {
        var sqlBase = @"UPDATE produto
                        SET nome = @nome, preco = @preco,
                            quantidade_estoque = @qtd
                        WHERE id = @id;";

        ExecuteNonQuery(sqlBase,
            new MySqlParameter("@nome", produto.Nome),
            new MySqlParameter("@preco", produto.Preco),
            new MySqlParameter("@qtd", produto.QuantidadeEstoque),
            new MySqlParameter("@id", produto.Id));

        var sqlFilha = @"UPDATE produto_industrializado
                         SET marca = @marca, codigo_barras = @codigo
                         WHERE id = @id;";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@marca", produto.Marca),
            new MySqlParameter("@codigo", (object?)produto.CodigoBarras ?? DBNull.Value),
            new MySqlParameter("@id", produto.Id));
    }

    // ----------------------------- DELETE -----------------------------
    public void Deletar(int id)
    {
        var sql = "DELETE FROM produto WHERE id = @id;";
        ExecuteNonQuery(sql, new MySqlParameter("@id", id));
    }
}
