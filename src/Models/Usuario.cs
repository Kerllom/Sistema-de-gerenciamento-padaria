namespace PadariaApp.Models;

// Mesma logica do Produto: nenhum usuario e um "usuario generico",
// todo usuario E cliente OU funcionario.
public abstract class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;

    // ATENCAO: aqui guarda-se o HASH da senha, nunca a senha em texto
    // puro. A geracao do hash sera feita antes de salvar no banco.
    public string Senha { get; set; } = string.Empty;

    protected Usuario() { }

    protected Usuario(string nome, string email, string login, string senha)
    {
        Nome = nome;
        Email = email;
        Login = login;
        Senha = senha;
    }

    // Metodo polimorfico: cada tipo de usuario exibe seu perfil de
    // um jeito diferente.
    public virtual string ExibirPerfil()
    {
        return $"#{Id} - {Nome} | Email: {Email} | Login: {Login}";
    }
}
