using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPadaria
{
    internal class ProdutoMenu
    {
        ProdutoRepositorio ProdutoRep = new ProdutoRepositorio();
        public void Main()
        {
            Console.WriteLine("╔=========================╗");
            Console.WriteLine("║    Tipos de produto:    ║");
            Console.WriteLine("║                         ║");
            Console.WriteLine("║ [1] - Perecíveis        ║");
            Console.WriteLine("║ [2] - Industrializados  ║");
            Console.WriteLine("║ [0] - Voltar            ║");
            Console.WriteLine("╚=========================╝");
            int Tipo = int.Parse(Console.ReadLine());

            switch (Tipo)
            {
                case 0:
                    return;

                case 1:
                    Pereciveis();
                    break;

                case 2:
                     Industrializados(); 
                    break;

            }
        }

        public void Pereciveis()
        {
            Console.Write("Nome: ");
            string Nome = Console.ReadLine();
            Console.Write("Preco: ");
            decimal Preco = decimal.Parse(Console.ReadLine());
            Console.Write("Quantidade: ");
            int Quantidade = int.Parse(Console.ReadLine());
            Console.Write("Data de validade (dd/mm/aaaa): ");
            DateTime DataValidade = DateTime.Parse(Console.ReadLine());
            Console.Write("Refrigerado (true/false): ");
            bool Refrigerado = bool.Parse(Console.ReadLine());

            ProdutoPerecivel perecivel = new ProdutoPerecivel
            {
                Nome = Nome,
                Preco = Preco,
                QuantidadeEstoque = Quantidade,
                Tipo = "PERECIVEL",
                DataValidade = DataValidade,
                Refrigerado = Refrigerado
            };

            ProdutoRep.AdicionarProdutoPerecivel(perecivel);
            Console.WriteLine("Produto adicionado com sucesso!");

        }

        public void Industrializados()
        {
            Console.Write("Nome: ");
            string Nome = Console.ReadLine();
            Console.Write("Preco: ");
            decimal Preco = decimal.Parse(Console.ReadLine());
            Console.Write("Quantidade: ");
            int Quantidade = int.Parse(Console.ReadLine());
            Console.Write("Marca: ");
            string Marca = Console.ReadLine();
            Console.Write("Código de barras: ");
            string CodigoBarras = Console.ReadLine();

            ProdutoIndustrializado industrializado = new ProdutoIndustrializado
            {
                Nome = Nome,
                Preco = Preco,
                QuantidadeEstoque = Quantidade,
                Tipo = "INDUSTRIALIZADO",
                Marca = Marca,
                CodigoBarras = CodigoBarras
            };

            ProdutoRep.AdicionarProdutoIndustrializado(industrializado);
            Console.WriteLine("Produto adicionado com sucesso!");

        }
    }
}
