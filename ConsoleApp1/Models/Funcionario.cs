using System;

namespace Padaria.Models
{
    // O ": Usuario" significa que o Funcionario herda tudo de Usuario
    public class Funcionario : Usuario
    {
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }

        public Funcionario(int id, string nome, string email, string login, string senha, string cargo, decimal salario, DateTime dataAdmissao)
            : base(id, nome, email, login, senha)
        {
            Cargo = cargo;
            Salario = salario;
            DataAdmissao = dataAdmissao;
        }

        public override string ExibirPerfil()
        {
            return base.ExibirPerfil() + $" | Cargo: {Cargo}";
        }
    }
}