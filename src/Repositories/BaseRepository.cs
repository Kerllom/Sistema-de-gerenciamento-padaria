using MySql.Data.MySqlClient;
using PadariaApp.Data;

namespace PadariaApp.Repositories;

// CLASSE BASE DOS REPOSITORIOS.
//
// Por que ela existe (defenda na apresentacao):
//   Todos os repositorios (Produto, Usuario, etc) abrem conexao,
//   executam comando e fecham conexao. Se cada repositorio tivesse seu
//   proprio jeito de fazer isso, viraria duplicacao de codigo - o que
//   o documento da atividade penaliza explicitamente.
//
//   Aqui, concentramos essas operacoes comuns. Os repositorios filhos
//   so escrevem o SQL especifico deles e chamam estes metodos.
//
// "abstract" = ninguem cria um BaseRepository "puro"; ele so existe
// para ser herdado.
public abstract class BaseRepository
{
    // Executa um INSERT/UPDATE/DELETE.
    // "params" deixa chamar passando varios parametros sem criar array.
    // Retorna o numero de linhas afetadas.
    protected int ExecuteNonQuery(string sql, params MySqlParameter[] parametros)
    {
        // "using" = a conexao e o comando sao fechados AUTOMATICAMENTE
        // ao sair deste bloco, mesmo se der erro. Boa pratica do C#.
        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);

        // Adiciona os parametros ao comando. ISSO E O PREPARED STATEMENT:
        // o valor digitado pelo usuario NUNCA vira parte do SQL bruto.
        // Imune a SQL Injection.
        comando.Parameters.AddRange(parametros);

        return comando.ExecuteNonQuery();
    }

    // Executa um INSERT e devolve o ID gerado pelo AUTO_INCREMENT.
    // Necessario para o caso da heranca: depois de inserir na tabela
    // base (produto/usuario), precisamos do ID gerado para inserir na
    // tabela filha (produto_perecivel/cliente/etc).
    protected int ExecuteInsertAndGetId(string sql, params MySqlParameter[] parametros)
    {
        using var conexao = DatabaseConnection.GetConnection();
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddRange(parametros);
        comando.ExecuteNonQuery();

        // LastInsertedId = o equivalente C# do LAST_INSERT_ID() do MySQL.
        return (int)comando.LastInsertedId;
    }
}
