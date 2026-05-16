# Diagrama de Classes — Sistema de Gerenciamento da Padaria

O sistema possui **duas hierarquias de herança**, atendendo ao requisito
de no mínimo três classes em hierarquia (o trabalho tem seis).

Todos os atributos são **privados** (`-`), acessados via getters/setters
no C# — aplicando encapsulamento de forma consistente.

```mermaid
classDiagram
    class Produto {
        -int id
        -string nome
        -decimal preco
        -int quantidadeEstoque
        +descricaoCompleta() string
    }

    class ProdutoPerecivel {
        -DateTime dataValidade
        -bool refrigerado
        +descricaoCompleta() string
    }

    class ProdutoIndustrializado {
        -string marca
        -string codigoBarras
        +descricaoCompleta() string
    }

    class Usuario {
        -int id
        -string nome
        -string email
        -string login
        -string senha
        +exibirPerfil() string
    }

    class Cliente {
        -DateTime dataCadastro
        -int pontosFidelidade
        +exibirPerfil() string
    }

    class Funcionario {
        -string cargo
        -decimal salario
        -DateTime dataAdmissao
        +exibirPerfil() string
    }

    Produto <|-- ProdutoPerecivel
    Produto <|-- ProdutoIndustrializado
    Usuario <|-- Cliente
    Usuario <|-- Funcionario
```

## Como ler

- As setas `<|--` representam **herança** (a subclasse "é um" tipo da
  classe base). Ex: `ProdutoPerecivel` herda de `Produto`.
- Cada subclasse **sobrescreve** o método da classe base
  (`descricaoCompleta` ou `exibirPerfil`), demonstrando **polimorfismo**.
- Os atributos com `-` são privados (encapsulamento). No C#, são
  acessados por meio de propriedades (`get`/`set`).

## Mapeamento para o banco

Cada hierarquia vira **3 tabelas** no MySQL (uma para a classe base e
uma para cada subclasse), ligadas pelo `id` via `FOREIGN KEY`.
Ver detalhes em `docs/contrato-banco.md`.
