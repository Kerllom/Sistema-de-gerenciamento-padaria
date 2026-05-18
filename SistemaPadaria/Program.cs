using SistemaPadaria;

ProdutoRepositorio ProdutoRep = new ProdutoRepositorio();
ProdutoMenu Menu = new ProdutoMenu();
ProdutoEdit ProdutoEdit = new ProdutoEdit();

while (true)
{
    Console.WriteLine("╔=========================╗");
    Console.WriteLine("║     Sistema Padaria     ║");
    Console.WriteLine("║                         ║");
    Console.WriteLine("║ [1] - Adicionar Produto ║");
    Console.WriteLine("║ [2] - Listar Produtos   ║");
    Console.WriteLine("║ [3] - Atualizar Produto ║");
    Console.WriteLine("║ [4] - Excluir Produto   ║");
    Console.WriteLine("║ [0] - Sair              ║");
    Console.WriteLine("╚=========================╝");

    Console.Write("\nEscolha uma opção: ");
    int Opcao = int.Parse(Console.ReadLine());

        switch (Opcao)
        {
            case 0:
                Console.WriteLine("FINALIZANDO...");
                return;

            case 1:
                Menu.Main();
                
                break;

            case 2:
            List<Produto> produtos = ProdutoRep.ListarProdutos();
            foreach (Produto p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Preco: {p.Preco} | Tipo: {p.Tipo}");
            }

            Console.WriteLine("\nPRESSIONE 'ENTER' PARA CONTINUAR...");
            Console.ReadLine();

            break;

        case 3:
            ProdutoEdit.AtualizarProduto();
            break;

        case 4:
            Console.Write("Digite o ID do produto: ");
            int Id = int.Parse(Console.ReadLine());
            ProdutoRep.ExcluirProduto(Id);
            Console.WriteLine("Produto excluído!");

            Console.WriteLine("\nPRESSIONE 'ENTER' PARA CONTINUAR...");
            Console.ReadLine();

            break;

    }
}