# Sistema de Agendamento para Salão de Beleza

## Objetivo

Sistema web para gerenciamento de agendamentos de uma profissional autônoma.

O objetivo é substituir a agenda física utilizada atualmente.

O foco do projeto é:

1. Resolver uma necessidade real.
2. Aplicar boas práticas de arquitetura no ecossistema .NET.

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
* Result Pattern

## Testes

* xUnit

## Infraestrutura

* Docker
* Docker Compose
* GitHub Actions

---

# Arquitetura

## CQRS Manual

Não utilizar MediatR.

Cada caso de uso deve possuir:

* Command + Handler

ou

* Query + Handler

Handlers devem ser registrados explicitamente via Dependency Injection.

---

# Result Pattern

Não utilizar exceções para fluxo de negócio.

Todo handler deve retornar:

* Result
* Result<T>

Exceções apenas para problemas inesperados ou de infraestrutura.

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

Seguir rigorosamente a arquitetura definida no SDD.

---

# Domínio Inicial

## Usuario

Representa o administrador do sistema.

Não existe suporte para múltiplos usuários nesta versão.

---

## Agendamento

Representa um horário reservado para atendimento.

Campos:

* ClienteNome
* ClienteTelefone
* Servico
* ValorServico
* DataHoraInicio
* DataHoraFim
* Status
* Observacao

---

# Regras de Negócio

* Não permitir horários sobrepostos.
* Não permitir horários no passado.
* DataHoraFim deve ser maior que DataHoraInicio.
* Agendamentos cancelados não bloqueiam horários.
* ClienteNome é obrigatório.
* ValorServico deve ser maior que zero.

---

# Autenticação

Utilizar JWT Bearer Authentication.

O sistema possui apenas um usuário administrador.

Não implementar:

* Registro de usuário
* Recuperação de senha
* Perfis
* Roles

Todos os endpoints devem exigir autenticação, exceto:

POST /api/auth/login

---

# Queries

Obrigatório:

* AsNoTracking()
* DTO Projection

Proibido:

* Modificar estado
* Retornar entidades

---

# Commands

Responsáveis por:

* Aplicar regras
* Persistir alterações

---

# Entity Framework

Não utilizar Repository Genérico.

Não criar abstrações preventivas.

Criar apenas abstrações que possuam uso real.

---

# Validação

Utilizar FluentValidation.

Todo Command deve possuir Validator.

---

# Testes

Prioridade:

1. Regras de negócio
2. Command Handlers
3. Query Handlers

Evitar testes decorativos.

---

# O que NÃO fazer

* Não usar MediatR.
* Não usar AutoMapper.
* Não usar Repository Genérico.
* Não usar exceções para fluxo.
* Não colocar regra de negócio em Controllers.
* Não retornar entidades em Queries.
* Não criar abstrações sem necessidade.
* Não implementar funcionalidades fora do escopo da V1.

---

# Fluxo para Agentes

Antes de qualquer implementação:

1. Ler docs/PRD.md
2. Ler docs/SDD.md
3. Ler este documento
4. Ler todo o repositório
5. Identificar padrões existentes
6. Seguir os padrões encontrados
7. Questionar ambiguidades antes de implementar

Nunca assumir regras de negócio sem confirmação.

Sempre apresentar um plano antes de gerar código.

Antes de iniciar qualquer implementação, leia integralmente o repositório e identifique os padrões já existentes. Não introduza novos padrões arquiteturais sem justificativa técnica clara.
