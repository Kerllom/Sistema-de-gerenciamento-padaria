using System;

namespace SistemaPadaria
{
    internal class Cliente : Usuario
    {
        public DateTime DataCadastro { get; set; }
        public int PontosFidelidade { get; set; }

        // ----------------------------------------------------------------
        // POLIMORFISMO: sobrescreve o metodo da classe-mae (Usuario)
        // usando 'override'. Quando chamamos ExibirDetalhes() em um
        // objeto Cliente, ESTA versao roda no lugar da versao do pai.
        // ----------------------------------------------------------------
        public override string ExibirDetalhes()
        {
            return $"ID: {Id} | Nome: {Nome} | Email: {Email} | " +
                   $"CLIENTE | Cadastrado em: {DataCadastro:dd/MM/yyyy} | " +
                   $"Pontos: {PontosFidelidade}";
        }
    }
}