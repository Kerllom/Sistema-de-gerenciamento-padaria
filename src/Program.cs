using PadariaApp.Data;

Console.WriteLine("=== Sistema de Gerenciamento da Padaria ===");
Console.WriteLine();
Console.WriteLine("Testando conexao com o banco de dados...");

if (DatabaseConnection.TestarConexao())
{
    Console.WriteLine("[OK] Conexao com o MySQL estabelecida com sucesso!");
    Console.WriteLine();
    Console.WriteLine("Banco de dados pronto para uso.");
}
else
{
    Console.WriteLine("[FALHA] Nao foi possivel conectar ao banco.");
    Console.WriteLine("Verifique:");
    Console.WriteLine("  1. O MySQL esta rodando?");
    Console.WriteLine("  2. O arquivo .env existe na raiz do projeto?");
    Console.WriteLine("  3. As credenciais no .env estao corretas?");
    Console.WriteLine("  4. O banco 'padaria_db' existe?");
}

Console.WriteLine();
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();
