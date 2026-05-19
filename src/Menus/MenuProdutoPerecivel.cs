using PadariaApp.Models;
using PadariaApp.Repositories;

namespace PadariaApp.Menus;

public static class MenuProdutoPerecivel
{
    // Repositorio criado uma vez e reusado pelas operacoes desta classe.
    private static readonly ProdutoPerecivelRepository _repo = new();

    public static void Exibir()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== PRODUTOS PERECIVEIS ===");
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
        Console.WriteLine("=== CADASTRAR PRODUTO PERECIVEL ===");

        Console.Write("Nome: ");
        var nome = Console.ReadLine() ?? "";

        Console.Write("Preco (ex: 12,50): ");
        if (!decimal.TryParse(Console.ReadLine(), out var preco))
        {
            Console.WriteLine("Preco invalido.");
            Pausa();
            return;
        }

        Console.Write("Quantidade em estoque: ");
        if (!int.TryParse(Console.ReadLine(), out var qtd))
        {
            Console.WriteLine("Quantidade invalida.");
            Pausa();
            return;
        }

        Console.Write("Data de validade (dd/MM/yyyy): ");
        if (!DateTime.TryParse(Console.ReadLine(), out var validade))
        {
            Console.WriteLine("Data invalida.");
            Pausa();
            return;
        }

        Console.Write("Refrigerado? (s/n): ");
        var refrig = (Console.ReadLine() ?? "").Trim().ToLower() == "s";

        var produto = new ProdutoPerecivel(nome, preco, qtd, validade, refrig);
        var id = _repo.Inserir(produto);

        Console.WriteLine($"Cadastrado com sucesso. ID: {id}");
        Pausa();
    }

    private static void Listar()
    {
        Console.Clear();
        Console.WriteLine("=== PRODUTOS PERECIVEIS CADASTRADOS ===");
        Console.WriteLine();

        var lista = _repo.ListarTodos();
        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum produto perecivel cadastrado.");
        }
        else
        {
            // Polimorfismo: DescricaoCompleta() de cada item retorna
            // a versao de ProdutoPerecivel (com validade e refrigerado).
            foreach (var p in lista)
            {
                Console.WriteLine(p.DescricaoCompleta());
            }
        }
        Pausa();
    }

    private static void Atualizar()
    {
        Console.Clear();
        Console.WriteLine("=== ATUALIZAR PRODUTO PERECIVEL ===");

        Console.Write("ID do produto a atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID invalido.");
            Pausa();
            return;
        }

        var produto = _repo.BuscarPorId(id);
        if (produto == null)
        {
            Console.WriteLine("Produto nao encontrado.");
            Pausa();
            return;
        }

        Console.WriteLine($"Atual: {produto.DescricaoCompleta()}");
        Console.WriteLine("(Deixe vazio para manter o valor atual)");
        Console.WriteLine();

        Console.Write($"Novo nome [{produto.Nome}]: ");
        var nome = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nome)) produto.Nome = nome;

        Console.Write($"Novo preco [{produto.Preco}]: ");
        var precoTexto = Console.ReadLine();
        if (decimal.TryParse(precoTexto, out var preco)) produto.Preco = preco;

        Console.Write($"Nova quantidade [{produto.QuantidadeEstoque}]: ");
        var qtdTexto = Console.ReadLine();
        if (int.TryParse(qtdTexto, out var qtd)) produto.QuantidadeEstoque = qtd;

        Console.Write($"Nova data de validade [{produto.DataValidade:dd/MM/yyyy}]: ");
        var validadeTexto = Console.ReadLine();
        if (DateTime.TryParse(validadeTexto, out var validade)) produto.DataValidade = validade;

        Console.Write($"Refrigerado (s/n) [{(produto.Refrigerado ? "s" : "n")}]: ");
        var refrigTexto = (Console.ReadLine() ?? "").Trim().ToLower();
        if (refrigTexto == "s") produto.Refrigerado = true;
        else if (refrigTexto == "n") produto.Refrigerado = false;

        _repo.Atualizar(produto);
        Console.WriteLine("Atualizado com sucesso.");
        Pausa();
    }

    private static void Deletar()
    {
        Console.Clear();
        Console.WriteLine("=== DELETAR PRODUTO PERECIVEL ===");

        Console.Write("ID do produto a deletar: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("ID invalido.");
            Pausa();
            return;
        }

        var produto = _repo.BuscarPorId(id);
        if (produto == null)
        {
            Console.WriteLine("Produto nao encontrado.");
            Pausa();
            return;
        }

        Console.WriteLine($"Vai deletar: {produto.DescricaoCompleta()}");
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

    // Pequena funcao auxiliar para nao repetir o "pressione qualquer
    // tecla" em todas as operacoes. Evita duplicacao de codigo.
    private static void Pausa()
    {
        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
