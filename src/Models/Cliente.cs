namespace PadariaApp.Models;

public class Cliente : Usuario
{
    public DateTime DataCadastro { get; set; }
    public int PontosFidelidade { get; set; }

    public Cliente() : base() { }

    public Cliente(string nome, string email, string login, string senha,
                   DateTime dataCadastro, int pontosFidelidade)
        : base(nome, email, login, senha)
    {
        DataCadastro = dataCadastro;
        PontosFidelidade = pontosFidelidade;
    }

    public override string ExibirPerfil()
    {
        return base.ExibirPerfil() +
               $" | Cliente desde: {DataCadastro:dd/MM/yyyy}" +
               $" | Pontos: {PontosFidelidade}";
    }
}
