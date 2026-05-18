using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaPadaria
{
    internal class ProdutoRepositorio
    {
        Conexao conexao = new Conexao();

        public void AdicionarProduto(Produto produto)
        {
            // Chama o metodo que retorna a conexão 
            MySqlConnection conn = conexao.GetConexao();

            // Abre a conexão
            conn.Open();

            string sql = "INSERT INTO produto (nome, preco, quantidade_estoque, tipo) VALUES (@nome, @preco, @quantidade, @tipo)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", produto.Nome);
            cmd.Parameters.AddWithValue("@preco", produto.Preco);
            cmd.Parameters.AddWithValue("@quantidade", produto.QuantidadeEstoque);
            cmd.Parameters.AddWithValue("@tipo", produto.Tipo);
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void AdicionarProdutoPerecivel(ProdutoPerecivel produtoPerecivel)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "INSERT INTO produto (nome, preco, quantidade_estoque, tipo) VALUES (@nome, @preco, @quantidade, @tipo)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", produtoPerecivel.Nome);
            cmd.Parameters.AddWithValue("@preco", produtoPerecivel.Preco);
            cmd.Parameters.AddWithValue("@quantidade", produtoPerecivel.QuantidadeEstoque);
            cmd.Parameters.AddWithValue("@tipo", produtoPerecivel.Tipo);
            cmd.ExecuteNonQuery();

            long idGerado = cmd.LastInsertedId;
            
            sql = "INSERT INTO produto_perecivel (id, data_validade, refrigerado) VALUES (@idGerado, @data,@refrigerado)";
            MySqlCommand cmd2 = new MySqlCommand(sql, conn);           
            cmd2.Parameters.AddWithValue("idGerado", idGerado);
            cmd2.Parameters.AddWithValue("@data", produtoPerecivel.DataValidade);
            cmd2.Parameters.AddWithValue("@refrigerado", produtoPerecivel.Refrigerado);
            cmd2.ExecuteNonQuery();

            conn.Close();

        }

        public void AdicionarProdutoIndustrializado(ProdutoIndustrializado produtoIndustrializado)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "INSERT INTO produto (nome, preco, quantidade_estoque, tipo) VALUES (@nome, @preco, @quantidade, @tipo)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", produtoIndustrializado.Nome);
            cmd.Parameters.AddWithValue("@preco", produtoIndustrializado.Preco);
            cmd.Parameters.AddWithValue("@quantidade", produtoIndustrializado.QuantidadeEstoque);
            cmd.Parameters.AddWithValue("@tipo", produtoIndustrializado.Tipo);
            cmd.ExecuteNonQuery();

            long idGerado = cmd.LastInsertedId;

            sql = "INSERT INTO produto_industrializado (id, marca, codigo_barras) VALUES (@idGerado, @marca, @codigo)";
            MySqlCommand cmd2 = new MySqlCommand(sql, conn);
            cmd2.Parameters.AddWithValue("idGerado", idGerado);
            cmd2.Parameters.AddWithValue("@marca", produtoIndustrializado.Marca);
            cmd2.Parameters.AddWithValue("@codigo", produtoIndustrializado.CodigoBarras);
            cmd2.ExecuteNonQuery();

            conn.Close();

        }

        public List<Produto> ListarProdutos()
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "SELECT * FROM produto";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            // MySqlDataReader lê os resultados linha por linha
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Produto> produtos = new List<Produto>();

            while (reader.Read()) 
            {
                Produto produto1 = new Produto
                {
                    Id =reader.GetInt32("id"),
                    Nome = reader.GetString("nome"),
                    Preco = reader.GetDecimal("preco"),
                    QuantidadeEstoque = reader.GetInt32("quantidade_estoque"),
                    Tipo = reader.GetString("tipo")
                   
                };

                produtos.Add(produto1);
            }

            conn.Close();

            return produtos;
        }

        public void ExcluirProduto(int id)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "DELETE FROM produto WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand( sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void AtualizarProduto(Produto produto1)
        {
            MySqlConnection conn = conexao.GetConexao();
            conn.Open();

            string sql = "UPDATE produto SET nome = @nome, preco = @preco, quantidade_estoque = @quantidade, tipo = @tipo WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nome", produto1.Nome);
            cmd.Parameters.AddWithValue("@preco", produto1.Preco);
            cmd.Parameters.AddWithValue("@quantidade", produto1.QuantidadeEstoque);
            cmd.Parameters.AddWithValue("@tipo", produto1.Tipo);
            cmd.Parameters.AddWithValue("@id", produto1.Id);

            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
