using System;

namespace SistemaPadaria
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; }

        // ----------------------------------------------------------------
        // POLIMORFISMO: este metodo e marcado como 'virtual', o que
        // significa que as subclasses (Cliente, Funcionario) podem
        // SOBRESCREVE-LO usando a palavra 'override' e dar a sua propria
        // versao deste comportamento.
        // ----------------------------------------------------------------
        public virtual string ExibirDetalhes()
        {
            return $"ID: {Id} | Nome: {Nome} | Email: {Email} | Tipo: {Tipo}";
        }
    }
}