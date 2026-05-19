using MySql.Data.MySqlClient;
using PadariaApp.Data;
using PadariaApp.Models;

namespace PadariaApp.Repositories;

// CRUD de Cliente. Mesma logica do ProdutoPerecivelRepository:
// cada cliente vira duas linhas no banco (usuario + cliente),
// ligadas pelo mesmo id.
public class ClienteRepository : BaseRepository
{
    // ----------------------------- CREATE -----------------------------
    public int Inserir(Cliente cliente)
    {
        // Passo 1: insere na tabela base 'usuario' e captura o id gerado.
        var sqlBase = @"INSERT INTO usuario (nome, email, login, senha, tipo)
                        VALUES (@nome, @email, @login, @senha, 'CLIENTE');";

        var idGerado = ExecuteInsertAndGetId(sqlBase,
            new MySqlParameter("@nome", cliente.Nome),
            new MySqlParameter("@email", cliente.Email),
            new MySqlParameter("@login", cliente.Login),
            new MySqlParameter("@senha", cliente.Senha));

        // Passo 2: insere na tabela filha usando o mesmo id.
        var sqlFilha = @"INSERT INTO cliente (id, data_cadastro, pontos_fidelidade)
                         VALUES (@id, @cadastro, @pontos);";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@id", idGerado),
            new MySqlParameter("@cadastro", cliente.DataCadastro),
            new MySqlParameter("@pontos", cliente.PontosFidelidade));

        cliente.Id = idGerado;
        return idGerado;
    }

    // ------------------------------ READ ------------------------------
    public List<Cliente> ListarTodos()
    {
        var sql = @"SELECT u.id, u.nome, u.email, u.login, u.senha,
                           c.data_cadastro, c.pontos_fidelidade
                    FROM usuario u
                    INNER JOIN cliente c ON u.id = c.id;";

        var lista = new List<Cliente>();

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        using var leitor = comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(new Cliente
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nome"),
                Email = leitor.GetString("email"),
                Login = leitor.GetString("login"),
                Senha = leitor.GetString("senha"),
                DataCadastro = leitor.GetDateTime("data_cadastro"),
                PontosFidelidade = leitor.GetInt32("pontos_fidelidade")
            });
        }

        return lista;
    }

    public Cliente? BuscarPorId(int id)
    {
        var sql = @"SELECT u.id, u.nome, u.email, u.login, u.senha,
                           c.data_cadastro, c.pontos_fidelidade
                    FROM usuario u
                    INNER JOIN cliente c ON u.id = c.id
                    WHERE u.id = @id;";

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@id", id);

        using var leitor = comando.ExecuteReader();
        if (!leitor.Read()) return null;

        return new Cliente
        {
            Id = leitor.GetInt32("id"),
            Nome = leitor.GetString("nome"),
            Email = leitor.GetString("email"),
            Login = leitor.GetString("login"),
            Senha = leitor.GetString("senha"),
            DataCadastro = leitor.GetDateTime("data_cadastro"),
            PontosFidelidade = leitor.GetInt32("pontos_fidelidade")
        };
    }

    // ----------------------------- UPDATE -----------------------------
    public void Atualizar(Cliente cliente)
    {
        var sqlBase = @"UPDATE usuario
                        SET nome = @nome, email = @email,
                            login = @login, senha = @senha
                        WHERE id = @id;";

        ExecuteNonQuery(sqlBase,
            new MySqlParameter("@nome", cliente.Nome),
            new MySqlParameter("@email", cliente.Email),
            new MySqlParameter("@login", cliente.Login),
            new MySqlParameter("@senha", cliente.Senha),
            new MySqlParameter("@id", cliente.Id));

        var sqlFilha = @"UPDATE cliente
                         SET data_cadastro = @cadastro,
                             pontos_fidelidade = @pontos
                         WHERE id = @id;";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@cadastro", cliente.DataCadastro),
            new MySqlParameter("@pontos", cliente.PontosFidelidade),
            new MySqlParameter("@id", cliente.Id));
    }

    // ----------------------------- DELETE -----------------------------
    public void Deletar(int id)
    {
        // Apaga so da tabela base. A linha de 'cliente' some junto
        // por causa do ON DELETE CASCADE.
        var sql = "DELETE FROM usuario WHERE id = @id;";
        ExecuteNonQuery(sql, new MySqlParameter("@id", id));
    }
}
