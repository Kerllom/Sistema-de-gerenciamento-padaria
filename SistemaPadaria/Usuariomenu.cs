using System;

namespace SistemaPadaria
{
    internal class UsuarioMenu
    {
        UsuarioRepositorio UsuarioRep = new UsuarioRepositorio();

        public void Main()
        {
            Console.WriteLine("╔=========================╗");
            Console.WriteLine("║    Tipos de usuario:    ║");
            Console.WriteLine("║                         ║");
            Console.WriteLine("║ [1] - Cliente           ║");
            Console.WriteLine("║ [2] - Funcionario       ║");
            Console.WriteLine("║ [0] - Voltar            ║");
            Console.WriteLine("╚=========================╝");
            int Tipo = int.Parse(Console.ReadLine());

            switch (Tipo)
            {
                case 0:
                    return;

                case 1:
                    CadastrarCliente();
                    break;

                case 2:
                    CadastrarFuncionario();
                    break;
            }
        }

        public void CadastrarCliente()
        {
            Console.Write("Nome: ");
            string Nome = Console.ReadLine();
            Console.Write("Email: ");
            string Email = Console.ReadLine();
            Console.Write("Login: ");
            string Login = Console.ReadLine();
            Console.Write("Senha: ");
            string Senha = Console.ReadLine();

            Cliente cliente = new Cliente
            {
                Nome = Nome,
                Email = Email,
                Login = Login,
                Senha = Senha,
                Tipo = "CLIENTE",
                DataCadastro = DateTime.Now,
                PontosFidelidade = 0
            };

            UsuarioRep.AdicionarCliente(cliente);
            Console.WriteLine("Cliente cadastrado com sucesso!");
        }

        public void CadastrarFuncionario()
        {
            Console.Write("Nome: ");
            string Nome = Console.ReadLine();
            Console.Write("Email: ");
            string Email = Console.ReadLine();
            Console.Write("Login: ");
            string Login = Console.ReadLine();
            Console.Write("Senha: ");
            string Senha = Console.ReadLine();
            Console.Write("Cargo: ");
            string Cargo = Console.ReadLine();
            Console.Write("Salario: ");
            decimal Salario = decimal.Parse(Console.ReadLine());
            Console.Write("Data de admissao (dd/mm/aaaa): ");
            DateTime DataAdmissao = DateTime.Parse(Console.ReadLine());

            Funcionario funcionario = new Funcionario
            {
                Nome = Nome,
                Email = Email,
                Login = Login,
                Senha = Senha,
                Tipo = "FUNCIONARIO",
                Cargo = Cargo,
                Salario = Salario,
                DataAdmissao = DataAdmissao
            };

            UsuarioRep.AdicionarFuncionario(funcionario);
            Console.WriteLine("Funcionario cadastrado com sucesso!");
        }
    }
}