using MySql.Data.MySqlClient;
using PadariaApp.Data;
using PadariaApp.Models;

namespace PadariaApp.Repositories;

// CRUD de Funcionario. Mesma logica do ClienteRepository,
// so muda os campos especificos.
public class FuncionarioRepository : BaseRepository
{
    // ----------------------------- CREATE -----------------------------
    public int Inserir(Funcionario funcionario)
    {
        var sqlBase = @"INSERT INTO usuario (nome, email, login, senha, tipo)
                        VALUES (@nome, @email, @login, @senha, 'FUNCIONARIO');";

        var idGerado = ExecuteInsertAndGetId(sqlBase,
            new MySqlParameter("@nome", funcionario.Nome),
            new MySqlParameter("@email", funcionario.Email),
            new MySqlParameter("@login", funcionario.Login),
            new MySqlParameter("@senha", funcionario.Senha));

        var sqlFilha = @"INSERT INTO funcionario (id, cargo, salario, data_admissao)
                         VALUES (@id, @cargo, @salario, @admissao);";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@id", idGerado),
            new MySqlParameter("@cargo", funcionario.Cargo),
            new MySqlParameter("@salario", funcionario.Salario),
            new MySqlParameter("@admissao", funcionario.DataAdmissao));

        funcionario.Id = idGerado;
        return idGerado;
    }

    // ------------------------------ READ ------------------------------
    public List<Funcionario> ListarTodos()
    {
        var sql = @"SELECT u.id, u.nome, u.email, u.login, u.senha,
                           f.cargo, f.salario, f.data_admissao
                    FROM usuario u
                    INNER JOIN funcionario f ON u.id = f.id;";

        var lista = new List<Funcionario>();

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        using var leitor = comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(new Funcionario
            {
                Id = leitor.GetInt32("id"),
                Nome = leitor.GetString("nome"),
                Email = leitor.GetString("email"),
                Login = leitor.GetString("login"),
                Senha = leitor.GetString("senha"),
                Cargo = leitor.GetString("cargo"),
                Salario = leitor.GetDecimal("salario"),
                DataAdmissao = leitor.GetDateTime("data_admissao")
            });
        }

        return lista;
    }

    public Funcionario? BuscarPorId(int id)
    {
        var sql = @"SELECT u.id, u.nome, u.email, u.login, u.senha,
                           f.cargo, f.salario, f.data_admissao
                    FROM usuario u
                    INNER JOIN funcionario f ON u.id = f.id
                    WHERE u.id = @id;";

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@id", id);

        using var leitor = comando.ExecuteReader();
        if (!leitor.Read()) return null;

        return new Funcionario
        {
            Id = leitor.GetInt32("id"),
            Nome = leitor.GetString("nome"),
            Email = leitor.GetString("email"),
            Login = leitor.GetString("login"),
            Senha = leitor.GetString("senha"),
            Cargo = leitor.GetString("cargo"),
            Salario = leitor.GetDecimal("salario"),
            DataAdmissao = leitor.GetDateTime("data_admissao")
        };
    }

    // Util para o login do sistema: buscar por login.
    public Funcionario? BuscarPorLogin(string login)
    {
        var sql = @"SELECT u.id, u.nome, u.email, u.login, u.senha,
                           f.cargo, f.salario, f.data_admissao
                    FROM usuario u
                    INNER JOIN funcionario f ON u.id = f.id
                    WHERE u.login = @login;";

        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@login", login);

        using var leitor = comando.ExecuteReader();
        if (!leitor.Read()) return null;

        return new Funcionario
        {
            Id = leitor.GetInt32("id"),
            Nome = leitor.GetString("nome"),
            Email = leitor.GetString("email"),
            Login = leitor.GetString("login"),
            Senha = leitor.GetString("senha"),
            Cargo = leitor.GetString("cargo"),
            Salario = leitor.GetDecimal("salario"),
            DataAdmissao = leitor.GetDateTime("data_admissao")
        };
    }

    // ----------------------------- UPDATE -----------------------------
    public void Atualizar(Funcionario funcionario)
    {
        var sqlBase = @"UPDATE usuario
                        SET nome = @nome, email = @email,
                            login = @login, senha = @senha
                        WHERE id = @id;";

        ExecuteNonQuery(sqlBase,
            new MySqlParameter("@nome", funcionario.Nome),
            new MySqlParameter("@email", funcionario.Email),
            new MySqlParameter("@login", funcionario.Login),
            new MySqlParameter("@senha", funcionario.Senha),
            new MySqlParameter("@id", funcionario.Id));

        var sqlFilha = @"UPDATE funcionario
                         SET cargo = @cargo, salario = @salario,
                             data_admissao = @admissao
                         WHERE id = @id;";

        ExecuteNonQuery(sqlFilha,
            new MySqlParameter("@cargo", funcionario.Cargo),
            new MySqlParameter("@salario", funcionario.Salario),
            new MySqlParameter("@admissao", funcionario.DataAdmissao),
            new MySqlParameter("@id", funcionario.Id));
    }

    // ----------------------------- DELETE -----------------------------
    public void Deletar(int id)
    {
        var sql = "DELETE FROM usuario WHERE id = @id;";
        ExecuteNonQuery(sql, new MySqlParameter("@id", id));
    }
}
