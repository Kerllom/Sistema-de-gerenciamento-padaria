namespace PadariaApp.Models;

// O perfil "Admin" da padaria - quem gerencia produtos e estoque.
public class Funcionario : Usuario
{
    public string Cargo { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public DateTime DataAdmissao { get; set; }

    public Funcionario() : base() { }

    public Funcionario(string nome, string email, string login, string senha,
                       string cargo, decimal salario, DateTime dataAdmissao)
        : base(nome, email, login, senha)
    {
        Cargo = cargo;
        Salario = salario;
        DataAdmissao = dataAdmissao;
    }

    public override string ExibirPerfil()
    {
        return base.ExibirPerfil() +
               $" | Cargo: {Cargo}" +
               $" | Admitido em: {DataAdmissao:dd/MM/yyyy}";
        // Nota: salario nao aparece no perfil exibido (boa pratica).
    }
}
