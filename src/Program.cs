using PadariaApp.Data;
using PadariaApp.Menus;

// =====================================================================
// PONTO DE ENTRADA DO PROGRAMA.
//
// Responsabilidade unica: garantir que o banco esta acessivel e
// transferir o controle para o menu principal. Toda a logica de
// interacao com o usuario fica nos arquivos da pasta Menus/.
// =====================================================================

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Iniciando o Sistema de Gerenciamento da Padaria...");
Console.WriteLine("Verificando conexao com o banco de dados...");
Console.WriteLine();

if (!DatabaseConnection.TestarConexao())
{
    Console.WriteLine("[ERRO] Nao foi possivel conectar ao banco de dados.");
    Console.WriteLine("Verifique:");
    Console.WriteLine("  - O MySQL esta rodando?");
    Console.WriteLine("  - O arquivo .env existe na raiz do projeto?");
    Console.WriteLine("  - O banco 'padaria_db' existe?");
    Console.WriteLine();
    Console.WriteLine("Pressione qualquer tecla para sair...");
    Console.ReadKey();
    return;
}

// Tudo certo - entrega o controle ao menu principal.
MenuPrincipal.Exibir();
