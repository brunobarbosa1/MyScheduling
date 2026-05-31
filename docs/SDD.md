# SDD - Software Design Document

## Objetivo

Definir a arquitetura técnica do backend da aplicação.

---

# Stack

## Backend

* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* PostgreSQL
* FluentValidation
* JWT Authentication
* Swagger/OpenAPI

## Testes

* xUnit

## Infraestrutura

* Docker
* GitHub Actions

---

# Arquitetura

## CQRS Manual

Sem MediatR.

Cada caso de uso deve possuir:

* Command
* Handler

ou

* Query
* Handler

Handlers registrados explicitamente via Dependency Injection.

---

# Result Pattern

Todos os handlers devem retornar:

* Result
* Result<T>

Não utilizar exceções para fluxo de negócio.

Exceções devem ser reservadas para falhas inesperadas ou problemas de infraestrutura.

---

# Estrutura da Solução

MyScheduling.slnx

src/

Core/

* MyScheduling.Common
* MyScheduling.DependencyInjection

Modules/

* MyScheduling.Domain
* MyScheduling.Data
* MyScheduling.Service
* MyScheduling.Application
* MyScheduling.Presentations

UI/

* MyScheduling.API

tests/

* MyScheduling.Tests

---

# Domínio

## Usuario

Representa o administrador da aplicação.

Campos:

* Id
* Nome
* Email
* PasswordHash
* CreatedAt
* UpdatedAt

---

## Agendamento

Campos:

* Id
* ClienteNome
* ClienteTelefone
* Servico
* ValorServico
* DataHoraInicio
* DataHoraFim
* Status
* Observacao
* CriadoEm
* AtualizadoEm

---

# StatusAgendamento

* Agendado
* Concluido
* Cancelado

---

# Commands

## Auth

* LoginUser

## Agendamento

* CreateAgendamento
* UpdateAgendamento
* CancelAgendamento
* CompleteAgendamento
* DeleteAgendamento

---

# Queries

## Agendamento

* GetAgendamentoById
* GetAgendamentos
* GetAgendamentosByDate

---

# Read Side

Obrigatório:

* AsNoTracking()
* DTO Projection
* Sem mutação de estado

Proibido:

* Retornar entidade
* Persistir dados
* Executar regras de negócio

---

# Write Side

Responsável por:

* Aplicar regras de negócio
* Persistir alterações
* Validar invariantes

---

# Controllers

Controllers devem permanecer finos.

Responsabilidades:

* Receber request
* Chamar handlers
* Converter Result em IActionResult

---

# Middleware

Middleware global para tratamento de erros.

Formato padrão:

ProblemDetails

---

# Persistência

Banco:

PostgreSQL

ORM:

Entity Framework Core

Migrations:

Entity Framework Migrations

---

# Autenticação

Estratégia:

JWT Bearer Authentication

Fluxo:

Login
↓
Validação das credenciais
↓
Geração do JWT
↓
Acesso aos endpoints protegidos

---

# Endpoints de Autenticação

POST /api/auth/login

Request:

{
"email": "[admin@salao.com](mailto:admin@salao.com)",
"password": "123456"
}

Response:

{
"accessToken": "jwt-token",
"expiresIn": 3600
}

---

# Usuário Inicial

O sistema possuirá apenas um usuário administrador.

O usuário será criado automaticamente por migration ou seed inicial.

Não haverá cadastro de usuários na V1.

---

# Validação

Utilizar FluentValidation.

Todo Command deve possuir Validator próprio.

---

# Testes

Prioridade:

1. Entidades
2. Command Handlers
3. Query Handlers

Evitar testes decorativos.

---

# CI/CD

Pipeline mínima:

* Restore
* Build
* Test
* Publish
