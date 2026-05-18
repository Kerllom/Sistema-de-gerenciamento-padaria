using System;
using System.IO;
using System.Collections.Generic;
using MySqlConnector;

namespace Padaria.Database
{
    public static class ConnectionFactory
    {
        private static string _connectionString;

        static ConnectionFactory()
        {
            LoadEnv();

            // Monta a string de conexão baseada no seu .env.example
            string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            string database = Environment.GetEnvironmentVariable("DB_NAME") ?? "padaria_db";
            string user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Rafaelrez10@";

            _connectionString = $"Server={host};Port={port};Database={database};User ID={user};Password={password};";
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        private static void LoadEnv()
        {
            string root = Directory.GetCurrentDirectory();
            string dotenv = Path.Combine(root, ".env");

            if (!File.Exists(dotenv)) return;

            foreach (var line in File.ReadAllLines(dotenv))
            {
                var parts = line.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && !line.StartsWith("#"))
                {
                    Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
                }
            }
        }
    }
}