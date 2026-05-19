using MySql.Data.MySqlClient;
using PadariaApp.Data;
using PadariaApp.Models;

namespace PadariaApp.Repositories;

// CRUD de ProdutoPerecivel.
//
// CHAVE PARA ENTENDER: cada produto perecivel vira DUAS linhas no banco:
//   - uma em 'produto'           (com os campos comuns)
//   - uma em 'produto_perecivel' (com os campos especificos)
// ligadas pelo MESMO id. E o repositorio que coordena essa dupla.
public class ProdutoPerecivelRepository : BaseRepository
{
    // ----------------------------- CREATE -----------------------------
    public int Inserir(ProdutoPerecivel produto)
    {
        // Passo 1: insere na tabela base 'produto' e captura o id gerado.
        var sqlBase = @"INSERT INTO produto (nome, preco, quantidade_estoque, tipo)
                        VALUES (@nome, @preco, @qtd, 'PERECIVEL');";

        var idGerado = ExecuteInsertAndGetId(sqlBase,
            new MySqlParameter("@nome", produto.Nome),
            new MySqlParameter("@preco", produto.Preco),
            new MySqlParameter("@qtd", produto.QuantidadeEstoque));

        // Passo 2: insere na tabela filha usando o MESMO id.
        var sqlFilha = @"INSERT INTO produto_perecivel (id, data_validade, refrigerado)
                         VALUES (@id, @validade, @refrig);";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@id", idGerado),
            new MySqlParameter("@validade", produto.DataValidade),
            new MySqlParameter("@refrig", produto.Refrigerado));

        produto.Id = idGerado;
        return idGerado;
    }

    // ------------------------------ READ ------------------------------
    public List<ProdutoPerecivel> ListarTodos()
    {
        // JOIN une as duas tabelas pelo id, trazendo todos os campos.
        var sql = @"SELECT p.id, p.nome, p.preco, p.quantidade_estoque,
                           pp.data_validade, pp.refrigerado
                    FROM produto p
                    INNER JOIN produto_perecivel pp ON p.id = pp.id;";

        var lista = new List<ProdutoPerecivel>();

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        using var leitor = comando.ExecuteReader();

        // ExecuteReader devolve uma "tabela em memoria" - lemos linha
        // por linha com leitor.Read().
        while (leitor.Read())
        {
            lista.Add(new ProdutoPerecivel
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nome"),
                Preco = leitor.GetDecimal("preco"),
                QuantidadeEstoque = leitor.GetInt32("quantidade_estoque"),
                DataValidade = leitor.GetDateTime("data_validade"),
                Refrigerado = leitor.GetBoolean("refrigerado")
            });
        }

        return lista;
    }

    public ProdutoPerecivel? BuscarPorId(int id)
    {
        var sql = @"SELECT p.id, p.nome, p.preco, p.quantidade_estoque,
                           pp.data_validade, pp.refrigerado
                    FROM produto p
                    INNER JOIN produto_perecivel pp ON p.id = pp.id
                    WHERE p.id = @id;";

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@id", id);

        using var leitor = comando.ExecuteReader();
        if (!leitor.Read()) return null;  // nao encontrou

        return new ProdutoPerecivel
        {
            Id = leitor.GetInt32("id"),
            Nome = leitor.GetString("nome"),
            Preco = leitor.GetDecimal("preco"),
            QuantidadeEstoque = leitor.GetInt32("quantidade_estoque"),
            DataValidade = leitor.GetDateTime("data_validade"),
            Refrigerado = leitor.GetBoolean("refrigerado")
        };
    }

    // ----------------------------- UPDATE -----------------------------
    public void Atualizar(ProdutoPerecivel produto)
    {
        // Atualiza a tabela base.
        var sqlBase = @"UPDATE produto
                        SET nome = @nome, preco = @preco,
                            quantidade_estoque = @qtd
                        WHERE id = @id;";

        ExecuteNonQuery(sqlBase,
            new MySqlParameter("@nome", produto.Nome),
            new MySqlParameter("@preco", produto.Preco),
            new MySqlParameter("@qtd", produto.QuantidadeEstoque),
            new MySqlParameter("@id", produto.Id));

        // Atualiza a tabela filha.
        var sqlFilha = @"UPDATE produto_perecivel
                         SET data_validade = @validade, refrigerado = @refrig
                         WHERE id = @id;";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@validade", produto.DataValidade),
            new MySqlParameter("@refrig", produto.Refrigerado),
            new MySqlParameter("@id", produto.Id));
    }

    // ----------------------------- DELETE -----------------------------
    public void Deletar(int id)
    {
        // So precisa apagar da tabela base. A linha da tabela filha
        // some junto automaticamente por causa do ON DELETE CASCADE
        // que voce configurou no script SQL. Lembra dessa decisao?
        var sql = "DELETE FROM produto WHERE id = @id;";
        ExecuteNonQuery(sql, new MySqlParameter("@id", id));
    }
}
