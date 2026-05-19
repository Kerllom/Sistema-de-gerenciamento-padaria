namespace PadariaApp.Models;

// ": Produto" = HERDA de Produto. ProdutoPerecivel "e um" Produto.
public class ProdutoPerecivel : Produto
{
    public DateTime DataValidade { get; set; }
    public bool Refrigerado { get; set; }

    public ProdutoPerecivel() : base() { }

    // O ": base(...)" chama o construtor da classe-mae.
    public ProdutoPerecivel(string nome, decimal preco, int quantidadeEstoque,
                            DateTime dataValidade, bool refrigerado)
        : base(nome, preco, quantidadeEstoque)
    {
        DataValidade = dataValidade;
        Refrigerado = refrigerado;
    }

    // "override" = estou SOBRESCREVENDO o metodo da classe-mae.
    // Mesmo nome, comportamento diferente = POLIMORFISMO na pratica.
    public override string DescricaoCompleta()
    {
        var refrig = Refrigerado ? "Sim" : "Nao";
        return base.DescricaoCompleta() +
               $" | Validade: {DataValidade:dd/MM/yyyy} | Refrigerado: {refrig}";
    }
}
