namespace PadariaApp.Models;

public class ProdutoIndustrializado : Produto
{
    public string Marca { get; set; } = string.Empty;
    public string? CodigoBarras { get; set; }  // "?" = pode ser null (opcional)

    public ProdutoIndustrializado() : base() { }

    public ProdutoIndustrializado(string nome, decimal preco, int quantidadeEstoque,
                                  string marca, string? codigoBarras)
        : base(nome, preco, quantidadeEstoque)
    {
        Marca = marca;
        CodigoBarras = codigoBarras;
    }

    public override string DescricaoCompleta()
    {
        var codigo = CodigoBarras ?? "sem codigo";
        return base.DescricaoCompleta() +
               $" | Marca: {Marca} | Cod. Barras: {codigo}";
    }
}
