using MySql.Data.MySqlClient;
using DotNetEnv;

namespace PadariaApp.Data;

// PONTO UNICO DE CONEXAO com o banco de dados.
//
// Toda parte do sistema que precisa falar com o MySQL passa por aqui.
// Vantagens dessa abordagem (defenda isso na apresentacao):
//   1. As credenciais ficam em UM lugar so (vindas do .env).
//   2. Se mudar de banco no futuro, mexe so neste arquivo.
//   3. Os repositorios nao precisam saber NADA de configuracao -
//      so chamam GetConnection() e recebem a conexao pronta.
//
// Isso e a SEPARACAO DE RESPONSABILIDADES exigida pelo documento.
public static class DatabaseConnection
{
    // "static bool" significa: variavel da classe (nao do objeto).
    // Existe uma so na memoria, compartilhada por todos.
    private static bool _envLoaded = false;

    // Carrega o arquivo .env so na PRIMEIRA vez que e necessario.
    // Isso evita ler o arquivo do disco repetidamente.
    private static void EnsureEnvLoaded()
    {
        if (_envLoaded) return;

        // Procura o .env subindo pelas pastas pais ate achar.
        // Necessario porque o programa pode ser executado de varios
        // diretorios diferentes (ex: src/, raiz do projeto, etc).
        Env.TraversePath().Load();
        _envLoaded = true;
    }

    // Monta a "connection string" - o texto que diz ao MySQL onde
    // conectar e com quais credenciais.
    private static string BuildConnectionString()
    {
        EnsureEnvLoaded();

        // Lê cada variavel do .env. Se nao existir, usa um valor padrao.
        var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
        var name = Environment.GetEnvironmentVariable("DB_NAME") ?? "padaria_db";
        var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
        var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

        return $"Server={host};Port={port};Database={name};" +
               $"User Id={user};Password={pass};";
    }

    // Funcao publica usada pelos repositorios.
    // Retorna uma conexao JA ABERTA e pronta para uso.
    public static MySqlConnection GetConnection()
    {
        var connection = new MySqlConnection(BuildConnectionString());
        connection.Open();
        return connection;
    }

    // Funcao auxiliar para testar a conexao na inicializacao do programa.
    // Retorna true se conseguiu conectar, false caso contrario.
    public static bool TestarConexao()
    {
        try
        {
            using var connection = GetConnection();
            return connection.State == System.Data.ConnectionState.Open;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERRO DE CONEXAO] {ex.Message}");
            return false;
        }
    }
}
