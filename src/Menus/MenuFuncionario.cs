using PadariaApp.Models;
using PadariaApp.Repositories;

namespace PadariaApp.Menus;

public static class MenuFuncionario
{
    private static readonly FuncionarioRepository _repo = new();

    public static void Exibir()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== FUNCIONARIOS ===");
            Console.WriteLine();
            Console.WriteLine("  1 - Cadastrar");
            Console.WriteLine("  2 - Listar todos");
            Console.WriteLine("  3 - Atualizar");
            Console.WriteLine("  4 - Deletar");
            Console.WriteLine("  0 - Voltar");
            Console.WriteLine();
            Console.Write("Escolha: ");

            switch (Console.ReadLine())
            {
                case "1": Cadastrar(); break;
                case "2": Listar(); break;
                case "3": Atualizar(); break;
                case "4": Deletar(); break;
                case "0": return;
                default:
                    Console.WriteLine("Opcao invalida.");
                    Pausa();
                    break;
            }
        }
    }

    private static void Cadastrar()
    {
        Console.Clear();
        Console.WriteLine("=== CADASTRAR FUNCIONARIO ===");

        Console.Write("Nome: ");
        var nome = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";

        Console.Write("Login: ");
        var login = Console.ReadLine() ?? "";

        Console.Write("Senha: ");
        var senha = Console.ReadLine() ?? "";

        Console.Write("Cargo: ");
        var cargo = Console.ReadLine() ?? "";

        Console.Write("Salario (ex: 2500,00): ");
        if (!decimal.TryParse(Console.ReadLine(), out var salario))
        {
            Console.WriteLine("Salario invalido.");
            Pausa();
            return;
        }

        Console.Write("Data de admissao (dd/MM/yyyy): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var admissao))
        {
            Console.WriteLine("Data invalida.");
            Pausa();
            return;
        }

        var funcionario = new Funcionario(nome, email, login, senha,
                                          cargo, salario, admissao);

        try
        {
            var id = _repo.Inserir(funcionario);
            Console.WriteLine($"Cadastrado com sucesso. ID: {id}");
        }
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062)
        {
            Console.WriteLine("Erro: email ou login ja cadastrado no sistema.");
        }
        Pausa();
    }

    private static void Listar()
    {
        Console.Clear();
        Console.WriteLine("=== FUNCIONARIOS CADASTRADOS ===");
        Console.WriteLine();

        var lista = _repo.ListarTodos();
        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum funcionario cadastrado.");
        }
        else
        {
            // Polimorfismo: ExibirPerfil() chama a versao de Funcionario.
            foreach (var f in lista)
            {
                Console.WriteLine(f.ExibirPerfil());
            }
        }
        Pausa();
    }

    private static void Atualizar()
    {
        Console.Clear();
        Console.WriteLine("=== ATUALIZAR FUNCIONARIO ===");

        Console.Write("ID do funcionario a atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID invalido.");
            Pausa();
            return;
        }

        var funcionario = _repo.BuscarPorId(id);
        if (funcionario == null)
        {
            Console.WriteLine("Funcionario nao encontrado.");
            Pausa();
            return;
        }

        Console.WriteLine($"Atual: {funcionario.ExibirPerfil()}");
        Console.WriteLine("(Deixe vazio para manter o valor atual)");
        Console.WriteLine();

        Console.Write($"Novo nome [{funcionario.Nome}]: ");
        var nome = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nome)) funcionario.Nome = nome;

        Console.Write($"Novo email [{funcionario.Email}]: ");
        var email = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(email)) funcionario.Email = email;

        Console.Write($"Novo login [{funcionario.Login}]: ");
        var login = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(login)) funcionario.Login = login;

        Console.Write("Nova senha (vazio para manter): ");
        var senha = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(senha)) funcionario.Senha = senha;

        Console.Write($"Novo cargo [{funcionario.Cargo}]: ");
        var cargo = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(cargo)) funcionario.Cargo = cargo;

        Console.Write($"Novo salario [{funcionario.Salario}]: ");
        if (decimal.TryParse(Console.ReadLine(), out var salario)) funcionario.Salario = salario;

        Console.Write($"Nova data de admissao [{funcionario.DataAdmissao:dd/MM/yyyy}]: ");
        if (DateTime.TryParse(Console.ReadLine(), out var admissao)) funcionario.DataAdmissao = admissao;

        try
        {
            _repo.Atualizar(funcionario);
            Console.WriteLine("Atualizado com sucesso.");
        }
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062)
        {
            Console.WriteLine("Erro: email ou login ja cadastrado em outro usuario.");
        }
        Pausa();
    }

    private static void Deletar()
    {
        Console.Clear();
        Console.WriteLine("=== DELETAR FUNCIONARIO ===");

        Console.Write("ID do funcionario a deletar: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID invalido.");
            Pausa();
            return;
        }

        var funcionario = _repo.BuscarPorId(id);
        if (funcionario == null)
        {
            Console.WriteLine("Funcionario nao encontrado.");
            Pausa();
            return;
        }

        Console.WriteLine($"Vai deletar: {funcionario.ExibirPerfil()}");
        Console.Write("Confirma? (s/n): ");
        if ((Console.ReadLine() ?? "").Trim().ToLower() != "s")
        {
            Console.WriteLine("Operacao cancelada.");
            Pausa();
            return;
        }

        _repo.Deletar(id);
        Console.WriteLine("Deletado com sucesso.");
        Pausa();
    }

    private static void Pausa()
    {
        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
