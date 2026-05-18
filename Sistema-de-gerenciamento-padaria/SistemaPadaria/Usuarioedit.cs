using System;

namespace SistemaPadaria
{
    internal class UsuarioEdit
    {
        UsuarioRepositorio UsuarioRep = new UsuarioRepositorio();

        public void AtualizarUsuario()
        {
            Console.Write("Digite o ID do usuario: ");
            int IdAtualizar = int.Parse(Console.ReadLine());
            Console.Write("Novo nome: ");
            string NovoNome = Console.ReadLine();
            Console.Write("Novo email: ");
            string NovoEmail = Console.ReadLine();
            Console.Write("Novo login: ");
            string NovoLogin = Console.ReadLine();

            Usuario usuarioAtualizado = new Usuario
            {
                Id = IdAtualizar,
                Nome = NovoNome,
                Email = NovoEmail,
                Login = NovoLogin
            };

            UsuarioRep.AtualizarUsuario(usuarioAtualizado);
            Console.WriteLine("Usuario atualizado!");
        }
    }
}