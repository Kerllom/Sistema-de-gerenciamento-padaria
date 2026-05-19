using PadariaApp.Models;
using PadariaApp.Repositories;

namespace PadariaApp.Menus;

public static class MenuCliente
{
    private static readonly ClienteRepository _repo = new();

    public static void Exibir()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== CLIENTES ===");
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
        Console.WriteLine("=== CADASTRAR CLIENTE ===");

        Console.Write("Nome: ");
        var nome = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        var email = Console.ReadLine() ?? "";

        Console.Write("Login: ");
        var login = Console.ReadLine() ?? "";

        Console.Write("Senha: ");
        var senha = Console.ReadLine() ?? "";

        // Cliente novo entra com data de cadastro de hoje e 0 pontos.
        var cliente = new Cliente(nome, email, login, senha,
                                  dataCadastro: DateTime.Today,
                                  pontosFidelidade: 0);

        try
        {
            var id = _repo.Inserir(cliente);
            Console.WriteLine($"Cadastrado com sucesso. ID: {id}");
        }
        catch (MySql.Data.MySqlClient.MySqlException ex) when (ex.Number == 1062)
        {
            // Erro 1062 = duplicate entry. Acontece se email/login ja existir
            // (sao UNIQUE no banco). Mensagem amigavel em vez de stack trace.
            Console.WriteLine("Erro: email ou login ja cadastrado no sistema.");
        }
        Pausa();
    }

    private static void Listar()
    {
        Console.Clear();
        Console.WriteLine("=== CLIENTES CADASTRADOS ===");
        Console.WriteLine();

        var lista = _repo.ListarTodos();
        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum cliente cadastrado.");
        }
        else
        {
            // Polimorfismo: ExibirPerfil() chama a versao de Cliente.
            foreach (var c in lista)
            {
                Console.WriteLine(c.ExibirPerfil());
            }
        }
        Pausa();
    }

    private static void Atualizar()
    {
        Console.Clear();
        Console.WriteLine("=== ATUALIZAR CLIENTE ===");

        Console.Write("ID do cliente a atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID invalido.");
            Pausa();
            return;
        }

        var cliente = _repo.BuscarPorId(id);
        if (cliente == null)
        {
            Console.WriteLine("Cliente nao encontrado.");
            Pausa();
            return;
        }

        Console.WriteLine($"Atual: {cliente.ExibirPerfil()}");
        Console.WriteLine("(Deixe vazio para manter o valor atual)");
        Console.WriteLine();

        Console.Write($"Novo nome [{cliente.Nome}]: ");
        var nome = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nome)) cliente.Nome = nome;

        Console.Write($"Novo email [{cliente.Email}]: ");
        var email = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(email)) cliente.Email = email;

        Console.Write($"Novo login [{cliente.Login}]: ");
        var login = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(login)) cliente.Login = login;

        Console.Write("Nova senha (vazio para manter): ");
        var senha = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(senha)) cliente.Senha = senha;

        Console.Write($"Novos pontos de fidelidade [{cliente.PontosFidelidade}]: ");
        if (int.TryParse(Console.ReadLine(), out var pontos)) cliente.PontosFidelidade = pontos;

        try
        {
            _repo.Atualizar(cliente);
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
        Console.WriteLine("=== DELETAR CLIENTE ===");

        Console.Write("ID do cliente a deletar: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID invalido.");
            Pausa();
            return;
        }

        var cliente = _repo.BuscarPorId(id);
        if (cliente == null)
        {
            Console.WriteLine("Cliente nao encontrado.");
            Pausa();
            return;
        }

        Console.WriteLine($"Vai deletar: {cliente.ExibirPerfil()}");
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
