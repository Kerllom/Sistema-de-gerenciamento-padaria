using PadariaApp.Models;
using PadariaApp.Repositories;

namespace PadariaApp.Menus;

public static class MenuProdutoIndustrializado
{
    private static readonly ProdutoIndustrializadoRepository _repo = new();

    public static void Exibir()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== PRODUTOS INDUSTRIALIZADOS ===");
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
        Console.WriteLine("=== CADASTRAR PRODUTO INDUSTRIALIZADO ===");

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

        Console.Write("Marca: ");
        var marca = Console.ReadLine() ?? "";

        Console.Write("Codigo de barras (opcional, Enter para vazio): ");
        var codigoTexto = Console.ReadLine();
        var codigo = string.IsNullOrWhiteSpace(codigoTexto) ? null : codigoTexto;

        var produto = new ProdutoIndustrializado(nome, preco, qtd, marca, codigo);
        var id = _repo.Inserir(produto);

        Console.WriteLine($"Cadastrado com sucesso. ID: {id}");
        Pausa();
    }

    private static void Listar()
    {
        Console.Clear();
        Console.WriteLine("=== PRODUTOS INDUSTRIALIZADOS CADASTRADOS ===");
        Console.WriteLine();

        var lista = _repo.ListarTodos();
        if (lista.Count == 0)
        {
            Console.WriteLine("Nenhum produto industrializado cadastrado.");
        }
        else
        {
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
        Console.WriteLine("=== ATUALIZAR PRODUTO INDUSTRIALIZADO ===");

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
        if (decimal.TryParse(Console.ReadLine(), out var preco)) produto.Preco = preco;

        Console.Write($"Nova quantidade [{produto.QuantidadeEstoque}]: ");
        if (int.TryParse(Console.ReadLine(), out var qtd)) produto.QuantidadeEstoque = qtd;

        Console.Write($"Nova marca [{produto.Marca}]: ");
        var marca = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(marca)) produto.Marca = marca;

        Console.Write($"Novo codigo de barras [{produto.CodigoBarras ?? "vazio"}]: ");
        var codigo = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(codigo)) produto.CodigoBarras = codigo;

        _repo.Atualizar(produto);
        Console.WriteLine("Atualizado com sucesso.");
        Pausa();
    }

    private static void Deletar()
    {
        Console.Clear();
        Console.WriteLine("=== DELETAR PRODUTO INDUSTRIALIZADO ===");

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

    private static void Pausa()
    {
        Console.WriteLine();
        Console.WriteLine("Pressione qualquer tecla para continuar...");
        Console.ReadKey();
    }
}
