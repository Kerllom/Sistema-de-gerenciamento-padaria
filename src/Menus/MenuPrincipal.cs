namespace PadariaApp.Menus;

// Menu raiz do sistema. Mostra as opcoes principais e delega para
// os menus especificos de cada entidade.
public static class MenuPrincipal
{
    public static void Exibir()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=================================================");
            Console.WriteLine("   SISTEMA DE GERENCIAMENTO DA PADARIA");
            Console.WriteLine("=================================================");
            Console.WriteLine();
            Console.WriteLine("  1 - Produtos Pereciveis");
            Console.WriteLine("  2 - Produtos Industrializados");
            Console.WriteLine("  3 - Clientes");
            Console.WriteLine("  4 - Funcionarios");
            Console.WriteLine("  0 - Sair");
            Console.WriteLine();
            Console.Write("Escolha uma opcao: ");

            var opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": MenuProdutoPerecivel.Exibir(); break;
                case "2": MenuProdutoIndustrializado.Exibir(); break;
                case "3": MenuCliente.Exibir(); break;
                case "4": MenuFuncionario.Exibir(); break;
                case "0":
                    Console.WriteLine("Encerrando o sistema. Ate logo!");
                    return;
                default:
                    Console.WriteLine("Opcao invalida. Pressione qualquer tecla...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}
