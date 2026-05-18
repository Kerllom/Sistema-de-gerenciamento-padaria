using System;

namespace Padaria.Models
{
    // O ": Usuario" significa que o Cliente herda tudo de Usuario
    public class Cliente : Usuario
    {
        public DateTime DataCadastro { get; set; }
        public int PontosFidelidade { get; set; }

        public Cliente(int id, string nome, string email, string login, string senha, DateTime dataCadastro, int pontosFidelidade)
            : base(id, nome, email, login, senha)
        {
            DataCadastro = dataCadastro;
            PontosFidelidade = pontosFidelidade;
        }

        public override string ExibirPerfil()
        {
            return base.ExibirPerfil() + $" | Cliente desde: {DataCadastro.ToShortDateString()} | Pontos: {PontosFidelidade}";
        }
    }
}