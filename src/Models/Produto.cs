namespace PadariaApp.Models;

// "abstract" = ninguem pode criar um Produto "puro", so subclasses.
// Isso reflete a realidade: todo produto na padaria E perecivel OU
// industrializado, nunca um "produto generico".
public abstract class Produto
{
    // PROPRIEDADES com { get; set; } = a forma C# de encapsular.
    // Equivale a ter um campo privado + getNome() + setNome() escritos
    // a mao, so que em uma linha. O acesso so e possivel por aqui.
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int QuantidadeEstoque { get; set; }

    // Construtor vazio (necessario para o repositorio reconstruir o
    // objeto ao ler do banco).
    protected Produto() { }

    // Construtor com parametros (usado ao criar um novo produto).
    protected Produto(string nome, decimal preco, int quantidadeEstoque)
    {
        Nome = nome;
        Preco = preco;
        QuantidadeEstoque = quantidadeEstoque;
    }

    // "virtual" = este metodo PODE ser sobrescrito pelas subclasses.
    // E o ponto-chave do POLIMORFISMO: cada subclasse vai dar a sua
    // propria versao de DescricaoCompleta().
    public virtual string DescricaoCompleta()
    {
        return $"#{Id} - {Nome} | R$ {Preco:F2} | Estoque: {QuantidadeEstoque}";
    }
}
