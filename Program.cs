using SistemaPadaria;

ProdutoRepositorio ProdutoRep = new ProdutoRepositorio();
ProdutoMenu MenuProduto = new ProdutoMenu();
ProdutoEdit EditProduto = new ProdutoEdit();

UsuarioRepositorio UsuarioRep = new UsuarioRepositorio();
UsuarioMenu MenuUsuario = new UsuarioMenu();
UsuarioEdit EditUsuario = new UsuarioEdit();

while (true)
{
    Console.WriteLine("╔=========================╗");
    Console.WriteLine("║     Sistema Padaria     ║");
    Console.WriteLine("║                         ║");
    Console.WriteLine("║ [1] - Produtos          ║");
    Console.WriteLine("║ [2] - Usuarios          ║");
    Console.WriteLine("║ [0] - Sair              ║");
    Console.WriteLine("╚=========================╝");

    Console.Write("\nEscolha uma opcao: ");
    int Opcao = int.Parse(Console.ReadLine());

    switch (Opcao)
    {
        case 0:
            Console.WriteLine("FINALIZANDO...");
            return;

        case 1:
            MenuProdutos();
            break;

        case 2:
            MenuUsuarios();
            break;
    }
}

// ====================================================================
// SUBMENU: PRODUTOS  (logica que ja existia, agora isolada)
// ====================================================================
void MenuProdutos()
{
    Console.WriteLine("╔==========================╗");
    Console.WriteLine("║        Produtos          ║");
    Console.WriteLine("║                          ║");
    Console.WriteLine("║ [1] - Adicionar          ║");
    Console.WriteLine("║ [2] - Listar             ║");
    Console.WriteLine("║ [3] - Atualizar          ║");
    Console.WriteLine("║ [4] - Excluir            ║");
    Console.WriteLine("║ [0] - Voltar             ║");
    Console.WriteLine("╚==========================╝");

    Console.Write("\nEscolha uma opcao: ");
    int Opcao = int.Parse(Console.ReadLine());

    switch (Opcao)
    {
        case 0:
            return;

        case 1:
            MenuProduto.Main();
            break;

        case 2:
            List<Produto> produtos = ProdutoRep.ListarProdutos();
            foreach (Produto p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Preco: {p.Preco} | Tipo: {p.Tipo}");
            }
            break;

        case 3:
            EditProduto.AtualizarProduto();
            break;

        case 4:
            Console.Write("Digite o ID do produto: ");
            int Id = int.Parse(Console.ReadLine());
            ProdutoRep.ExcluirProduto(Id);
            Console.WriteLine("Produto excluido!");
            break;
    }
}

// ====================================================================
// SUBMENU: USUARIOS  (novo)
// Note como o foreach usa u.ExibirDetalhes() - o C# escolhe a versao
// certa (Cliente ou Funcionario) automaticamente. ISSO E POLIMORFISMO.
// ====================================================================
void MenuUsuarios()
{
    Console.WriteLine("╔==========================╗");
    Console.WriteLine("║        Usuarios          ║");
    Console.WriteLine("║                          ║");
    Console.WriteLine("║ [1] - Cadastrar          ║");
    Console.WriteLine("║ [2] - Listar             ║");
    Console.WriteLine("║ [3] - Atualizar          ║");
    Console.WriteLine("║ [4] - Excluir            ║");
    Console.WriteLine("║ [0] - Voltar             ║");
    Console.WriteLine("╚==========================╝");

    Console.Write("\nEscolha uma opcao: ");
    int Opcao = int.Parse(Console.ReadLine());

    switch (Opcao)
    {
        case 0:
            return;

        case 1:
            MenuUsuario.Main();
            break;

        case 2:
            List<Usuario> usuarios = UsuarioRep.ListarUsuarios();
            foreach (Usuario u in usuarios)
            {
                // POLIMORFISMO em acao: u pode ser Cliente ou Funcionario,
                // e cada um responde a ExibirDetalhes() do seu jeito.
                Console.WriteLine(u.ExibirDetalhes());
            }
            break;

        case 3:
            EditUsuario.AtualizarUsuario();
            break;

        case 4:
            Console.Write("Digite o ID do usuario: ");
            int Id = int.Parse(Console.ReadLine());
            UsuarioRep.ExcluirUsuario(Id);
            Console.WriteLine("Usuario excluido!");
            break;
    }
}