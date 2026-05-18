
-- SISTEMA DE GERENCIAMENTO DE PADARIA
-- O sistema tem DUAS hierarquias de heranca:
--
--    Produto              Usuario
--  
-- Perecivel  Industrializado   Cliente   Funcionario
--
-- Banco relacional NAO tem heranca, entao cada hierarquia vira 3
-- tabelas: uma da classe base e uma de cada subclasse.

CREATE DATABASE IF NOT EXISTS padaria_db;
USE padaria_db;

-- HIERARQUIA 1: PRODUTO
-- TABELA BASE: produto  -- o que TODO produto tem.
CREATE TABLE produto (
    id INT AUTO_INCREMENT PRIMARY KEY,

    nome VARCHAR(100) NOT NULL,            -- texto ate 100 chars, obrigatorio
    preco DECIMAL(10,2) NOT NULL,          -- numero com 2 casas: ideal p/ dinheiro
    quantidade_estoque INT NOT NULL DEFAULT 0,  -- inteiro; vale 0 se nao informado

    -- tipo: 'PERECIVEL' ou 'INDUSTRIALIZADO'. Serve para o codigo C#
    tipo VARCHAR(20) NOT NULL
);

-- SUBCLASSE: produto_perecivel  -- so o que e especifico dela.
CREATE TABLE produto_perecivel (
    id INT PRIMARY KEY,
    data_validade DATE NOT NULL,
    refrigerado BOOLEAN NOT NULL DEFAULT FALSE,  -- verdadeiro/falso

    -- ON DELETE CASCADE: se o produto for apagado, esta linha some junto
    -- (evita lixo no banco e erros no Delete do CRUD).
    FOREIGN KEY (id) REFERENCES produto(id) ON DELETE CASCADE
);
-- SUBCLASSE: produto_industrializado
CREATE TABLE produto_industrializado (
    id INT PRIMARY KEY,
    marca VARCHAR(50) NOT NULL,
    codigo_barras VARCHAR(20),             -- sem NOT NULL = campo OPCIONAL
    FOREIGN KEY (id) REFERENCES produto(id) ON DELETE CASCADE
);

-- HIERARQUIA 2: USUARIO
-- Mesma tecnica da hierarquia de produto (3 tabelas, id compartilhado).
-- TABELA BASE: usuario  -- o que TODO usuario tem.
CREATE TABLE usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    -- UNIQUE: o banco recusa cadastrar um email/login repetido.
    -- Dois usuarios nao podem ter o mesmo email ou o mesmo login.
    email VARCHAR(100) NOT NULL UNIQUE,
    login VARCHAR(50) NOT NULL UNIQUE,
    -- senha: aqui guarda-se a senha JA EMBARALHADA (hash) pelo codigo
    -- C#, NUNCA a senha real. 255 chars cabem o resultado do hash.
    senha VARCHAR(255) NOT NULL,
    -- tipo: 'CLIENTE' ou 'FUNCIONARIO'. Mesma ideia do tipo do produto.
    tipo VARCHAR(20) NOT NULL
);
-- SUBCLASSE: cliente
CREATE TABLE cliente (
    id INT PRIMARY KEY,
    data_cadastro DATE NOT NULL,
    pontos_fidelidade INT NOT NULL DEFAULT 0,
    FOREIGN KEY (id) REFERENCES usuario(id) ON DELETE CASCADE
);
-- SUBCLASSE: funcionario  (o perfil "Admin" da padaria)
CREATE TABLE funcionario (
    id INT PRIMARY KEY,
    cargo VARCHAR(50) NOT NULL,
    salario DECIMAL(10,2) NOT NULL,
    data_admissao DATE NOT NULL,
    FOREIGN KEY (id) REFERENCES usuario(id) ON DELETE CASCADE
);
-- DADOS DE TESTE (opcional) -- ajuda a testar o CRUD logo de inicio.
-- ORDEM IMPORTA: primeiro a tabela base, depois a subclasse, porque a
-- subclasse precisa que o id ja exista na base.
-- LAST_INSERT_ID() devolve o id que o AUTO_INCREMENT acabou de gerar.

-- Produtos
INSERT INTO produto (nome, preco, quantidade_estoque, tipo)
VALUES ('Pao Frances', 0.75, 200, 'PERECIVEL');
INSERT INTO produto_perecivel (id, data_validade, refrigerado)
VALUES (LAST_INSERT_ID(), '2026-05-16', FALSE);

INSERT INTO produto (nome, preco, quantidade_estoque, tipo)
VALUES ('Refrigerante 2L', 8.50, 50, 'INDUSTRIALIZADO');
INSERT INTO produto_industrializado (id, marca, codigo_barras)
VALUES (LAST_INSERT_ID(), 'Marca Exemplo', '7891234567890');

-- Usuarios  (a senha aqui e so um exemplo; no sistema real seria o hash)
INSERT INTO usuario (nome, email, login, senha, tipo)
VALUES ('Maria Cliente', 'maria@email.com', 'maria', 'hash_exemplo', 'CLIENTE');
INSERT INTO cliente (id, data_cadastro, pontos_fidelidade)
VALUES (LAST_INSERT_ID(), '2026-05-14', 0);

INSERT INTO usuario (nome, email, login, senha, tipo)
VALUES ('Joao Admin', 'joao@email.com', 'joao', 'hash_exemplo', 'FUNCIONARIO');
INSERT INTO funcionario (id, cargo, salario, data_admissao)
VALUES (LAST_INSERT_ID(), 'Gerente', 3000.00, '2026-01-10');
