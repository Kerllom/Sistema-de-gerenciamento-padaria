using System;

namespace Padaria.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }

        public Usuario(int id, string nome, string email, string login, string senha)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Login = login;
            Senha = senha;
        }

        public virtual string ExibirPerfil()
        {
            return $"Nome: {Nome} | Email: {Email}";
        }
    }
}