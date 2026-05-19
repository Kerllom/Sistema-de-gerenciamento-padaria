# Sistema de Gerenciamento da Padaria

Sistema de gerenciamento em console desenvolvido em C# com integração ao banco de dados MySQL, aplicando os três pilares da orientação a objetos: **herança**, **polimorfismo** e **encapsulamento**.

O sistema permite o cadastro, consulta, atualização e exclusão (CRUD) de produtos e usuários da padaria, organizados em duas hierarquias de classes.

---

## Funcionalidades

- CRUD completo de **Produtos Perecíveis** (pães, bolos — com data de validade e refrigeração)
- CRUD completo de **Produtos Industrializados** (bebidas, embalados — com marca e código de barras)
- CRUD completo de **Clientes** (com data de cadastro e pontos de fidelidade)
- CRUD completo de **Funcionários** (com cargo, salário e data de admissão)
- Menu interativo em console com navegação por números
- Validação de entrada e tratamento de erros (ex: email/login duplicado)
- Conexão configurável via arquivo `.env` (credenciais nunca vão para o repositório)

---

## Tecnologias

- **C#** com **.NET 8**
- **MySQL 8** como banco de dados relacional
- **MySql.Data** (driver oficial do MySQL para .NET)
- **DotNetEnv** (leitura de variáveis de ambiente do arquivo `.env`)

---

## Estrutura do projeto

```
Sistema-de-gerenciamento-padaria/
├── .env.example              # Modelo de configuração (vai para o repositório)
├── .gitignore                # Lista de arquivos ignorados pelo Git
├── README.md
├── database/
│   └── criar_banco_padaria.sql   # Script de criação do banco e tabelas
├── docs/
│   ├── diagrama-classes.md       # Diagrama UML em Mermaid
│   └── contrato-banco.md         # Mapeamento das classes para as tabelas
└── src/
    ├── PadariaApp.csproj
    ├── Program.cs                # Ponto de entrada
    ├── Data/
    │   └── DatabaseConnection.cs # Ponto único de conexão com o MySQL
    ├── Models/                   # Classes de domínio
    │   ├── Produto.cs
    │   ├── ProdutoPerecivel.cs
    │   ├── ProdutoIndustrializado.cs
    │   ├── Usuario.cs
    │   ├── Cliente.cs
    │   └── Funcionario.cs
    ├── Repositories/             # Camada de acesso a dados (DAO)
    │   ├── BaseRepository.cs
    │   ├── ProdutoPerecivelRepository.cs
    │   ├── ProdutoIndustrializadoRepository.cs
    │   ├── ClienteRepository.cs
    │   └── FuncionarioRepository.cs
    └── Menus/                    # Camada de apresentação (console)
        ├── MenuPrincipal.cs
        ├── MenuProdutoPerecivel.cs
        ├── MenuProdutoIndustrializado.cs
        ├── MenuCliente.cs
        └── MenuFuncionario.cs
```

A estrutura segue o princípio da **separação de responsabilidades**: as classes de domínio (Models) não conhecem o banco; o banco é acessado exclusivamente pela camada de repositório (Repositories); e a interação com o usuário fica isolada na camada de menus.

---

## Pré-requisitos

Antes de rodar o projeto, é necessário ter instalado:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- [MySQL 8](https://dev.mysql.com/downloads/mysql/) (servidor rodando localmente)
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) (opcional, para gerenciar o banco visualmente)
- [Git](https://git-scm.com/) (para clonar o repositório)

---

## Como instalar e executar

### 1. Clonar o repositório

```bash
git clone https://github.com/Kerllom/Sistema-de-gerenciamento-padaria.git
cd Sistema-de-gerenciamento-padaria
```

### 2. Criar o banco de dados

Abra o MySQL Workbench, conecte-se ao MySQL local e execute o script:

```
database/criar_banco_padaria.sql
```

Esse script cria o banco `padaria_db` com as 6 tabelas necessárias e insere alguns registros de exemplo. Você também pode executá-lo por linha de comando:

```bash
mysql -u root -p < database/criar_banco_padaria.sql
```

### 3. Configurar as credenciais do banco

Na raiz do projeto, copie o arquivo `.env.example` e renomeie a cópia para `.env`. Preencha com os dados do seu MySQL local:

```
DB_HOST=localhost
DB_PORT=3306
DB_NAME=padaria_db
DB_USER=root
DB_PASSWORD=sua_senha_aqui
```

> O arquivo `.env` está listado no `.gitignore` e **nunca é enviado para o repositório**, garantindo que as credenciais permaneçam locais.

### 4. Restaurar dependências e executar

```bash
cd src
dotnet restore
dotnet run
```

O programa testará a conexão com o banco e abrirá o menu principal.

---

## Diagrama de classes

Veja o diagrama completo em [`docs/diagrama-classes.md`](docs/diagrama-classes.md). Visão geral:

```
        Produto  (abstrata)                Usuario  (abstrata)
       /         \                        /          \
ProdutoPerecivel  ProdutoIndustrializado  Cliente   Funcionario
```

O mapeamento detalhado entre classes C# e tabelas MySQL está em [`docs/contrato-banco.md`](docs/contrato-banco.md).

---

## Decisões de design

### Duas hierarquias em vez de uma

Foram modeladas duas hierarquias independentes (Produto e Usuário) porque representam conceitos completamente distintos no domínio da padaria. Cada uma demonstra herança, polimorfismo e encapsulamento de forma independente, cumprindo com folga o requisito de "no mínimo três classes em hierarquia".

### Classes-base abstratas

`Produto` e `Usuario` são declaradas como `abstract` porque, no domínio do problema, todo produto é necessariamente perecível ou industrializado, e todo usuário é necessariamente cliente ou funcionário. Não faz sentido instanciar a classe-base diretamente, e o C# garante isso em tempo de compilação.

### Polimorfismo via `virtual` / `override`

Cada classe-base define um método polimórfico (`DescricaoCompleta()` em Produto e `ExibirPerfil()` em Usuário) marcado como `virtual`. As subclasses sobrescrevem com `override`, retornando uma string que inclui os campos específicos de cada tipo. Esse mecanismo é usado nos menus de listagem.

### Encapsulamento via propriedades

Todos os atributos das classes são acessados via propriedades `{ get; set; }` — a forma idiomática do C# para getters/setters. Isso permite controle de acesso futuro (ex: adicionar validação no setter) sem mudar o contrato público das classes.

### Mapeamento da herança no banco relacional

Banco relacional não suporta herança nativamente. A estratégia usada foi **uma tabela para a classe-base e uma tabela para cada subclasse**, ligadas pelo mesmo `id` via `FOREIGN KEY`. Essa abordagem evita colunas nulas espalhadas, expressa a hierarquia de forma clara, e usa `ON DELETE CASCADE` para garantir que a exclusão da linha-pai apague automaticamente a linha-filha.

### Camada de repositório (DAO)

As classes de domínio não contêm SQL. Todo acesso ao banco é centralizado em classes de repositório, que herdam de `BaseRepository` para evitar duplicação de código de conexão. Os repositórios usam **prepared statements** em 100% das queries, prevenindo SQL Injection.

### Conexão única configurável

A classe `DatabaseConnection` é o ponto único onde as credenciais e a string de conexão são montadas. Ela lê do arquivo `.env`, que fica fora do repositório. Caso o sistema migre para outro banco no futuro, a mudança ocorre em um único arquivo.

### Menus em arquivos separados

A camada de apresentação foi quebrada em cinco arquivos (um por entidade + um menu raiz) seguindo o mesmo princípio de separação aplicado nos repositórios. Cada menu tem responsabilidade única e fica fácil de manter.

---

## Autor

**Kerllom Luis** — projeto desenvolvido como atividade prática da disciplina de Programação de Aplicativos no SENAI.
