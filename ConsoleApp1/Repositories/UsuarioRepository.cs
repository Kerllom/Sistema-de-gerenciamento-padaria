using MySqlConnector;
using Padaria.Database;
using Padaria.Models;
using System;

namespace Padaria.Repositories
{
    public class UsuarioRepository
    {
        // === CADASTROS (CREATE) ===

        public void CadastrarCliente(Cliente cliente)
        {
            using (var conn = ConnectionFactory.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. Insere na tabela base (usuario)
                        int usuarioId = InserirUsuarioBase(conn, transaction, cliente, "CLIENTE");

                        // 2. Insere na tabela filha (cliente) usando o ID gerado
                        string sqlFilha = @"INSERT INTO cliente (id, data_cadastro, pontos_fidelidade) 
                                            VALUES (@Id, @DataCadastro, @PontosFidelidade);";

                        using (var cmd = new MySqlCommand(sqlFilha, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", usuarioId);
                            cmd.Parameters.AddWithValue("@DataCadastro", cliente.DataCadastro);
                            cmd.Parameters.AddWithValue("@PontosFidelidade", cliente.PontosFidelidade);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        cliente.Id = usuarioId; // Atualiza o objeto na memória
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void CadastrarFuncionario(Funcionario func)
        {
            using (var conn = ConnectionFactory.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        int usuarioId = InserirUsuarioBase(conn, transaction, func, "FUNCIONARIO");

                        string sqlFilha = @"INSERT INTO funcionario (id, cargo, salario, data_admissao) 
                                            VALUES (@Id, @Cargo, @Salario, @DataAdmissao);";

                        using (var cmd = new MySqlCommand(sqlFilha, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Id", usuarioId);
                            cmd.Parameters.AddWithValue("@Cargo", func.Cargo);
                            cmd.Parameters.AddWithValue("@Salario", func.Salario);
                            cmd.Parameters.AddWithValue("@DataAdmissao", func.DataAdmissao);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        func.Id = usuarioId;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        // Método auxiliar privado para evitar duplicação (Regra dos menos de 30 linhas)
        private int InserirUsuarioBase(MySqlConnection conn, MySqlTransaction trans, Usuario user, string tipo)
        {
            string sqlBase = @"INSERT INTO usuario (nome, email, login, senha, tipo) 
                               VALUES (@Nome, @Email, @Login, @Senha, @Tipo);
                               SELECT LAST_INSERT_ID();";

            using (var cmd = new MySqlCommand(sqlBase, conn, trans))
            {
                cmd.Parameters.AddWithValue("@Nome", user.Nome);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Login", user.Login);
                cmd.Parameters.AddWithValue("@Senha", user.Senha); // Idealmente passar o hash
                cmd.Parameters.AddWithValue("@Tipo", tipo);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // === CONSULTAS (READ) ===

        public Cliente BuscarClientePorId(int id)
        {
            string sql = @"SELECT u.*, c.data_cadastro, c.pontos_fidelidade 
                           FROM usuario u 
                           INNER JOIN cliente c ON u.id = c.id 
                           WHERE u.id = @Id;";

            using (var conn = ConnectionFactory.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente(
                                reader.GetInt32("id"),
                                reader.GetString("nome"),
                                reader.GetString("email"),
                                reader.GetString("login"),
                                reader.GetString("senha"),
                                reader.GetDateTime("data_cadastro"),
                                reader.GetInt32("pontos_fidelidade")
                            );
                        }
                    }
                }
            }
            return null; // Retorna null se não encontrar
        }

        public Funcionario BuscarFuncionarioPorId(int id)
        {
            string sql = @"SELECT u.*, f.cargo, f.salario, f.data_admissao 
                           FROM usuario u 
                           INNER JOIN funcionario f ON u.id = f.id 
                           WHERE u.id = @Id;";

            using (var conn = ConnectionFactory.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Funcionario(
                                reader.GetInt32("id"),
                                reader.GetString("nome"),
                                reader.GetString("email"),
                                reader.GetString("login"),
                                reader.GetString("senha"),
                                reader.GetString("cargo"),
                                reader.GetDecimal("salario"),
                                reader.GetDateTime("data_admissao")
                            );
                        }
                    }
                }
            }
            return null;
        }

        // === EXCLUSÃO (DELETE) ===
        // Conforme o seu contrato-banco.md, o ON DELETE CASCADE apaga a tabela filha automaticamente!
        public bool DeletarUsuario(int id)
        {
            string sql = "DELETE FROM usuario WHERE id = @Id;";

            using (var conn = ConnectionFactory.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0;
                }
            }
        }
    }
}