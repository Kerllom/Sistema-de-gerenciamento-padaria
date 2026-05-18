using System;
using Padaria.Models;
using Padaria.Repositories;

namespace Padaria
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DA PADARIA ===");

            UsuarioRepository repo = new UsuarioRepository();

            try
            {
                Console.WriteLine("\n--- CADASTRAR NOVO FUNCIONÁRIO ---");

                // 1. O sistema faz a pergunta (Write) e guarda a resposta (ReadLine)
                Console.Write("Digite o Nome do funcionário: ");
                string nome = Console.ReadLine();

                Console.Write("Digite o Email: ");
                string email = Console.ReadLine();

                Console.Write("Digite o Login de acesso: ");
                string login = Console.ReadLine();

                Console.Write("Digite a Senha: ");
                string senha = Console.ReadLine();

                Console.Write("Digite o Cargo (ex: Padeiro, Caixa): ");
                string cargo = Console.ReadLine();

                Console.Write("Digite o Salário (ex: 2500,50): ");
                string textoSalario = Console.ReadLine();

                // Converte o texto do salário para um número decimal
                // Se a pessoa digitar letras sem querer, o salário fica 0 para não travar o programa
                decimal.TryParse(textoSalario, out decimal salario);

                Funcionario novoFunc = new Funcionario(
                    id: 0, // O banco gera sozinho
                    nome: nome,
                    email: email,
                    login: login,
                    senha: senha,
                    cargo: cargo,
                    salario: salario,
                    dataAdmissao: DateTime.Now // Pega a data e hora do exato momento
                );

                // 3. Salva no banco de dados MySQL
                repo.CadastrarFuncionario(novoFunc);
                Console.WriteLine($"\n Sucesso! {nome} foi salvo(a) no banco de dados com o ID: {novoFunc.Id}");

                Console.WriteLine("\n--- Lendo do Banco de Dados para confirmar ---");

                Usuario usuarioBuscado = repo.BuscarFuncionarioPorId(novoFunc.Id);

                if (usuarioBuscado != null)
                {
                    Console.WriteLine(usuarioBuscado.ExibirPerfil());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Ocorreu um erro no sistema!.");
                Console.WriteLine($"Detalhe do erro: {ex.Message}");
            }

            Console.WriteLine("\nAperte qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}