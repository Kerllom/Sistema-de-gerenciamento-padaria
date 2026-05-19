using PadariaApp.Data;
using PadariaApp.Models;
using PadariaApp.Repositories;

// =====================================================================
// PROGRAM.CS TEMPORARIO - testa os repositorios de Usuario.
// O menu interativo de verdade vem na Etapa 5.
// =====================================================================

Console.WriteLine("=== TESTE DOS REPOSITORIOS DE USUARIO ===");
Console.WriteLine();

if (!DatabaseConnection.TestarConexao())
{
    Console.WriteLine("Falha na conexao. Encerrando.");
    return;
}

var repoCliente = new ClienteRepository();
var repoFuncionario = new FuncionarioRepository();

// ---------- CREATE ----------
Console.WriteLine("--- CREATE ---");
var cliente = new Cliente(
    nome: "Ana Silva",
    email: "ana@email.com",
    login: "ana.silva",
    senha: "senha123",
    dataCadastro: DateTime.Today,
    pontosFidelidade: 0);
var idCliente = repoCliente.Inserir(cliente);
Console.WriteLine($"Inseriu cliente com id {idCliente}");

var funcionario = new Funcionario(
    nome: "Carlos Souza",
    email: "carlos@email.com",
    login: "carlos.souza",
    senha: "senha456",
    cargo: "Atendente",
    salario: 2200.00m,
    dataAdmissao: DateTime.Today);
var idFuncionario = repoFuncionario.Inserir(funcionario);
Console.WriteLine($"Inseriu funcionario com id {idFuncionario}");
Console.WriteLine();

// ---------- READ ----------
Console.WriteLine("--- READ (todos os clientes) ---");
foreach (var c in repoCliente.ListarTodos())
{
    // POLIMORFISMO: ExibirPerfil() de Cliente roda a versao especifica.
    Console.WriteLine(c.ExibirPerfil());
}
Console.WriteLine();

Console.WriteLine("--- READ (todos os funcionarios) ---");
foreach (var f in repoFuncionario.ListarTodos())
{
    Console.WriteLine(f.ExibirPerfil());
}
Console.WriteLine();

// ---------- UPDATE ----------
Console.WriteLine("--- UPDATE ---");
var clienteParaAtualizar = repoCliente.BuscarPorId(idCliente);
if (clienteParaAtualizar != null)
{
    clienteParaAtualizar.PontosFidelidade = 50;
    repoCliente.Atualizar(clienteParaAtualizar);
    Console.WriteLine($"Atualizou cliente. Novo estado:");
    Console.WriteLine(repoCliente.BuscarPorId(idCliente)!.ExibirPerfil());
}
Console.WriteLine();

// ---------- BUSCA POR LOGIN (util para login no menu) ----------
Console.WriteLine("--- BUSCA POR LOGIN ---");
var funcEncontrado = repoFuncionario.BuscarPorLogin("carlos.souza");
Console.WriteLine($"Busca por 'carlos.souza': {(funcEncontrado != null ? funcEncontrado.ExibirPerfil() : "nao encontrado")}");
Console.WriteLine();

// ---------- DELETE ----------
Console.WriteLine("--- DELETE ---");
repoCliente.Deletar(idCliente);
repoFuncionario.Deletar(idFuncionario);
Console.WriteLine($"Deletou cliente id {idCliente} e funcionario id {idFuncionario}");

var conferir = repoCliente.BuscarPorId(idCliente);
Console.WriteLine($"Buscar cliente deletado retornou: {(conferir == null ? "null (correto)" : "ainda existe!")}");

Console.WriteLine();
Console.WriteLine("=== TESTE CONCLUIDO ===");
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();
