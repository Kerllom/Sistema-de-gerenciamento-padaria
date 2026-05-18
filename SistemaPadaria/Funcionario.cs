using System;

namespace SistemaPadaria
{
    internal class Funcionario : Usuario
    {
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }

        // ----------------------------------------------------------------
        // POLIMORFISMO: sobrescreve o metodo da classe-mae.
        // Repare como a SAIDA e diferente da do Cliente, mesmo chamando
        // o mesmo metodo - e exatamente isso que demonstra polimorfismo.
        // ----------------------------------------------------------------
        public override string ExibirDetalhes()
        {
            return $"ID: {Id} | Nome: {Nome} | Email: {Email} | " +
                   $"FUNCIONARIO | Cargo: {Cargo} | Salario: R$ {Salario:F2} | " +
                   $"Admitido em: {DataAdmissao:dd/MM/yyyy}";
        }
    }
}