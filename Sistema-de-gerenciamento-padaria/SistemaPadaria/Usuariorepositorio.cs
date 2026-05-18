using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace SistemaPadaria
{
    internal class UsuarioRepositorio
    {
        Conexao conexao = new Conexao();

        // ================================================================
        // ADICIONAR um Cliente
        // Insere primeiro na tabela base (usuario), pega o id gerado
        // pelo AUTO_INCREMENT e insere na tabela filha (cliente).
        // ================================================================
        public void AdicionarCliente(Cliente cliente)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "INSERT INTO usuario (nome, email, login, senha, tipo) " +
                         "VALUES (@nome, @email, @login, @senha, @tipo)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", cliente.Nome);
            cmd.Parameters.AddWithValue("@email", cliente.Email);
            cmd.Parameters.AddWithValue("@login", cliente.Login);
            cmd.Parameters.AddWithValue("@senha", cliente.Senha);
            cmd.Parameters.AddWithValue("@tipo", cliente.Tipo);
            cmd.ExecuteNonQuery();

            long idGerado = cmd.LastInsertedId;

            sql = "INSERT INTO cliente (id, data_cadastro, pontos_fidelidade) " +
                  "VALUES (@idGerado, @data, @pontos)";
            MySqlCommand cmd2 = new MySqlCommand(sql, conn);
            cmd2.Parameters.AddWithValue("@idGerado", idGerado);
            cmd2.Parameters.AddWithValue("@data", cliente.DataCadastro);
            cmd2.Parameters.AddWithValue("@pontos", cliente.PontosFidelidade);
            cmd2.ExecuteNonQuery();

            conn.Close();
        }

        // ================================================================
        // ADICIONAR um Funcionario
        // Mesmo padrao do Cliente: primeiro a base, depois a filha.
        // ================================================================
        public void AdicionarFuncionario(Funcionario funcionario)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "INSERT INTO usuario (nome, email, login, senha, tipo) " +
                         "VALUES (@nome, @email, @login, @senha, @tipo)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
            cmd.Parameters.AddWithValue("@email", funcionario.Email);
            cmd.Parameters.AddWithValue("@login", funcionario.Login);
            cmd.Parameters.AddWithValue("@senha", funcionario.Senha);
            cmd.Parameters.AddWithValue("@tipo", funcionario.Tipo);
            cmd.ExecuteNonQuery();

            long idGerado = cmd.LastInsertedId;

            sql = "INSERT INTO funcionario (id, cargo, salario, data_admissao) " +
                  "VALUES (@idGerado, @cargo, @salario, @data)";
            MySqlCommand cmd2 = new MySqlCommand(sql, conn);
            cmd2.Parameters.AddWithValue("@idGerado", idGerado);
            cmd2.Parameters.AddWithValue("@cargo", funcionario.Cargo);
            cmd2.Parameters.AddWithValue("@salario", funcionario.Salario);
            cmd2.Parameters.AddWithValue("@data", funcionario.DataAdmissao);
            cmd2.ExecuteNonQuery();

            conn.Close();
        }

        // ================================================================
        // LISTAR usuarios
        // Faz JOIN para trazer os dados das tabelas filhas. Assim,
        // ao listar, cada usuario ja vem como Cliente ou Funcionario,
        // permitindo aproveitar o POLIMORFISMO no ExibirDetalhes().
        // ================================================================
        public List<Usuario> ListarUsuarios()
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = @"
                SELECT u.id, u.nome, u.email, u.login, u.tipo,
                       c.data_cadastro, c.pontos_fidelidade,
                       f.cargo, f.salario, f.data_admissao
                FROM usuario u
                LEFT JOIN cliente c ON u.id = c.id
                LEFT JOIN funcionario f ON u.id = f.id";

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Usuario> usuarios = new List<Usuario>();

            while (reader.Read())
            {
                string tipo = reader.GetString("tipo");

                if (tipo == "CLIENTE")
                {
                    Cliente cliente = new Cliente
                    {
                        Id = reader.GetInt32("id"),
                        Nome = reader.GetString("nome"),
                        Email = reader.GetString("email"),
                        Login = reader.GetString("login"),
                        Tipo = tipo,
                        DataCadastro = reader.GetDateTime("data_cadastro"),
                        PontosFidelidade = reader.GetInt32("pontos_fidelidade")
                    };
                    usuarios.Add(cliente);
                }
                else if (tipo == "FUNCIONARIO")
                {
                    Funcionario funcionario = new Funcionario
                    {
                        Id = reader.GetInt32("id"),
                        Nome = reader.GetString("nome"),
                        Email = reader.GetString("email"),
                        Login = reader.GetString("login"),
                        Tipo = tipo,
                        Cargo = reader.GetString("cargo"),
                        Salario = reader.GetDecimal("salario"),
                        DataAdmissao = reader.GetDateTime("data_admissao")
                    };
                    usuarios.Add(funcionario);
                }
            }

            conn.Close();
            return usuarios;
        }

        // ================================================================
        // EXCLUIR usuario
        // Apaga so da tabela base. O ON DELETE CASCADE no banco apaga
        // a linha da tabela filha (cliente OU funcionario) automaticamente.
        // ================================================================
        public void ExcluirUsuario(int id)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "DELETE FROM usuario WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        // ================================================================
        // ATUALIZAR dados basicos do usuario (tabela base)
        // ================================================================
        public void AtualizarUsuario(Usuario usuario)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "UPDATE usuario SET nome = @nome, email = @email, " +
                         "login = @login WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@email", usuario.Email);
            cmd.Parameters.AddWithValue("@login", usuario.Login);
            cmd.Parameters.AddWithValue("@id", usuario.Id);
            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}