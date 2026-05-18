# Contrato Banco ↔ Código

> **Por que este arquivo existe:** o código C# vai escrever comandos SQL
> usando os nomes exatos das tabelas e colunas. Se alguém escrever
> diferente do que está no banco, o programa quebra. Este documento é a
> **fonte da verdade** sobre os nomes. Se alguém quiser mudar algo,
> avisa o trio antes.

## Convenção de nomes

- **No banco** (MySQL): tudo `minusculo_com_underscore`. Ex: `quantidade_estoque`
- **No código** (C#): classes e propriedades em `PascalCase`. Ex: `QuantidadeEstoque`
- A camada de repositório (DAO) faz a tradução entre os dois mundos.

---

## Hierarquia 1 — Produto

### Tabela `produto` (classe base)

| Coluna               | Tipo MySQL      | Propriedade C#       | Tipo C#     | Notas                       |
|----------------------|-----------------|----------------------|-------------|-----------------------------|
| `id`                 | INT             | `Id`                 | `int`       | PK, AUTO_INCREMENT          |
| `nome`               | VARCHAR(100)    | `Nome`               | `string`    | obrigatório                 |
| `preco`              | DECIMAL(10,2)   | `Preco`              | `decimal`   | obrigatório                 |
| `quantidade_estoque` | INT             | `QuantidadeEstoque`  | `int`       | default 0                   |
| `tipo`               | VARCHAR(20)     | (uso interno do DAO) | —           | `'PERECIVEL'` ou `'INDUSTRIALIZADO'` |

### Tabela `produto_perecivel`

| Coluna          | Tipo MySQL  | Propriedade C#   | Tipo C#     | Notas                            |
|-----------------|-------------|------------------|-------------|----------------------------------|
| `id`            | INT         | `Id` (herdado)   | `int`       | PK + FK → `produto.id`           |
| `data_validade` | DATE        | `DataValidade`   | `DateTime`  | obrigatório                      |
| `refrigerado`   | BOOLEAN     | `Refrigerado`    | `bool`      | default `false`                  |

### Tabela `produto_industrializado`

| Coluna          | Tipo MySQL    | Propriedade C#   | Tipo C#  | Notas                       |
|-----------------|---------------|------------------|----------|-----------------------------|
| `id`            | INT           | `Id` (herdado)   | `int`    | PK + FK → `produto.id`      |
| `marca`         | VARCHAR(50)   | `Marca`          | `string` | obrigatório                 |
| `codigo_barras` | VARCHAR(20)   | `CodigoBarras`   | `string` | opcional (pode ser null)    |

---

## Hierarquia 2 — Usuario

### Tabela `usuario` (classe base)

| Coluna  | Tipo MySQL    | Propriedade C#       | Tipo C#  | Notas                          |
|---------|---------------|----------------------|----------|--------------------------------|
| `id`    | INT           | `Id`                 | `int`    | PK, AUTO_INCREMENT             |
| `nome`  | VARCHAR(100)  | `Nome`               | `string` | obrigatório                    |
| `email` | VARCHAR(100)  | `Email`              | `string` | obrigatório, **UNIQUE**        |
| `login` | VARCHAR(50)   | `Login`              | `string` | obrigatório, **UNIQUE**        |
| `senha` | VARCHAR(255)  | `Senha`              | `string` | guardar **hash**, nunca a senha real |
| `tipo`  | VARCHAR(20)   | (uso interno do DAO) | —        | `'CLIENTE'` ou `'FUNCIONARIO'` |

### Tabela `cliente`

| Coluna              | Tipo MySQL | Propriedade C#       | Tipo C#    | Notas                       |
|---------------------|------------|----------------------|------------|-----------------------------|
| `id`                | INT        | `Id` (herdado)       | `int`      | PK + FK → `usuario.id`      |
| `data_cadastro`     | DATE       | `DataCadastro`       | `DateTime` | obrigatório                 |
| `pontos_fidelidade` | INT        | `PontosFidelidade`   | `int`      | default 0                   |

### Tabela `funcionario`

| Coluna           | Tipo MySQL    | Propriedade C#   | Tipo C#    | Notas                       |
|------------------|---------------|------------------|------------|-----------------------------|
| `id`             | INT           | `Id` (herdado)   | `int`      | PK + FK → `usuario.id`      |
| `cargo`          | VARCHAR(50)   | `Cargo`          | `string`   | obrigatório                 |
| `salario`        | DECIMAL(10,2) | `Salario`        | `decimal`  | obrigatório                 |
| `data_admissao`  | DATE          | `DataAdmissao`   | `DateTime` | obrigatório                 |

---

## Regras importantes para a camada de repositório (DAO)

1. **Ordem ao INSERIR uma subclasse:** primeiro insere na tabela base
   (`produto` ou `usuario`), pega o `id` gerado (`LAST_INSERT_ID()` em
   MySQL), e só então insere na tabela filha usando esse mesmo `id`.

2. **Ao DELETAR**, basta apagar da tabela base. A linha da subclasse
   some junto automaticamente (`ON DELETE CASCADE`).

3. **Ao LER (SELECT)** uma subclasse específica, use `JOIN`:
   ```sql
   SELECT p.*, pp.data_validade, pp.refrigerado
   FROM produto p
   INNER JOIN produto_perecivel pp ON p.id = pp.id
   WHERE p.id = ?;
   ```

4. **SEMPRE** usar parâmetros preparados (`?` no MySQL, ou parâmetros
   nomeados no MySqlCommand do C#). **Nunca** concatenar string de
   entrada do usuário no SQL — evita SQL Injection.

5. **Sempre fechar a conexão** com o banco ao final de cada operação
   (usar `using` no C# resolve isso automaticamente).
