using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPadaria
{
    internal class ProdutoEdit
    {
        ProdutoRepositorio ProdutoRep = new ProdutoRepositorio();

        public void AtualizarProduto()
        {
            Console.WriteLine("Digite o id do produto: ");
            int IdAtualizar = int.Parse(Console.ReadLine());
            Console.Write("Novo nome: ");
            string NovoNome = Console.ReadLine();
            Console.Write("Novo preco: ");
            decimal NovoPreco = decimal.Parse(Console.ReadLine());
            Console.Write("Nova quantidade: ");
            int NovaQuantidade = int.Parse(Console.ReadLine());
            Console.Write("Novo tipo: ");
            string NovoTipo = Console.ReadLine();

            Produto produtoAtualizado = new Produto
            {
                Id = IdAtualizar,
                Nome = NovoNome,
                Preco = NovoPreco,
                QuantidadeEstoque = NovaQuantidade,
                Tipo = NovoTipo
            };

            ProdutoRep.AtualizarProduto(produtoAtualizado);
            Console.WriteLine("Produto Atualizado!");

            Console.WriteLine("\nPRESSIONE 'ENTER' PARA CONTINUAR...");
            Console.ReadLine();

        }
    }
}
