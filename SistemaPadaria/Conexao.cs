using DotNetEnv;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPadaria
{
    internal class Conexao
    {
        public MySqlConnection GetConexao() {
            // 1. Carrega o .env
            Env.Load(".env");

            string host = Env.GetString("DB_HOST");
            string port = Env.GetString("DB_PORT");
            string nome = Env.GetString("DB_NAME");
            string usuario = Env.GetString("DB_USER");
            string senha = Env.GetString("DB_PASSWORD");

            string connection = $"server={host};user id={usuario};database={nome};password={senha}; port={port};";

            return new MySqlConnection (connection);
        }
    }
}
