using PadariaApp.Data;
using PadariaApp.Models;
using PadariaApp.Repositories;

// =====================================================================
// PROGRAM.CS TEMPORARIO - so para testar os repositorios de Produto.
// O menu interativo de verdade vem na Etapa 5.
// =====================================================================

Console.WriteLine("=== TESTE DOS REPOSITORIOS DE PRODUTO ===");
Console.WriteLine();

if (!DatabaseConnection.TestarConexao())
{
    Console.WriteLine("Falha na conexao. Encerrando.");
    return;
}

var repoPerecivel = new ProdutoPerecivelRepository();
var repoIndustrializado = new ProdutoIndustrializadoRepository();

// ---------- CREATE ----------
Console.WriteLine("--- CREATE ---");
var bolo = new ProdutoPerecivel(
    nome: "Bolo de Cenoura",
    preco: 25.00m,
    quantidadeEstoque: 5,
    dataValidade: DateTime.Today.AddDays(3),
    refrigerado: false);
var idBolo = repoPerecivel.Inserir(bolo);
Console.WriteLine($"Inseriu perecivel com id {idBolo}");

var suco = new ProdutoIndustrializado(
    nome: "Suco de Laranja 1L",
    preco: 12.50m,
    quantidadeEstoque: 30,
    marca: "Del Vale",
    codigoBarras: "7891234500001");
var idSuco = repoIndustrializado.Inserir(suco);
Console.WriteLine($"Inseriu industrializado com id {idSuco}");
Console.WriteLine();

// ---------- READ (todos) ----------
Console.WriteLine("--- READ (todos os pereciveis) ---");
foreach (var p in repoPerecivel.ListarTodos())
{
    // Aqui acontece o POLIMORFISMO: chama DescricaoCompleta() na
    // referencia de Produto, mas roda a versao de ProdutoPerecivel.
    Console.WriteLine(p.DescricaoCompleta());
}
Console.WriteLine();

Console.WriteLine("--- READ (todos os industrializados) ---");
foreach (var p in repoIndustrializado.ListarTodos())
{
    Console.WriteLine(p.DescricaoCompleta());
}
Console.WriteLine();

// ---------- UPDATE ----------
Console.WriteLine("--- UPDATE ---");
var boloParaAtualizar = repoPerecivel.BuscarPorId(idBolo);
if (boloParaAtualizar != null)
{
    boloParaAtualizar.Preco = 28.00m;
    boloParaAtualizar.QuantidadeEstoque = 10;
    repoPerecivel.Atualizar(boloParaAtualizar);
    Console.WriteLine($"Atualizou bolo. Novo estado:");
    Console.WriteLine(repoPerecivel.BuscarPorId(idBolo)!.DescricaoCompleta());
}
Console.WriteLine();

// ---------- DELETE ----------
Console.WriteLine("--- DELETE ---");
repoPerecivel.Deletar(idBolo);
repoIndustrializado.Deletar(idSuco);
Console.WriteLine($"Deletou perecivel id {idBolo} e industrializado id {idSuco}");

var conferir = repoPerecivel.BuscarPorId(idBolo);
Console.WriteLine($"Buscar bolo deletado retornou: {(conferir == null ? "null (correto)" : "ainda existe!")}");

Console.WriteLine();
Console.WriteLine("=== TESTE CONCLUIDO ===");
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();
