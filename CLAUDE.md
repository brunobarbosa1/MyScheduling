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
* Google OAuth 2.0 (Authorization Code + PKCE)
* JWT Bearer Authentication (token interno emitido pelo backend)
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

Representa uma profissional cadastrada no sistema.

Autenticação exclusivamente via Google. Cada Usuario possui:

* Nome
* Email
* GoogleSub (identificador único imutável do Google)
* Papel (Usuario | Admin)

O sistema é multi-tenant: cada Usuario possui sua agenda isolada.

---

## Agendamento

Representa um horário reservado para atendimento.

Pertence a exatamente um Usuario (dono da agenda).

Campos:

* UsuarioId (FK)
* ClienteNome
* ClienteTelefone
* Servico
* ValorServico
* DataHoraInicio
* DataHoraFim
* Status
* TipoPagamento
* Observacao

---

# Regras de Negócio

* Não permitir horários sobrepostos dentro da agenda do mesmo Usuario (Usuarios distintos podem ter agendamentos no mesmo horário).
* Não permitir horários no passado.
* DataHoraFim deve ser maior que DataHoraInicio.
* Agendamentos cancelados não bloqueiam horários.
* ClienteNome é obrigatório.
* ValorServico é opcional e pode ser definido depois da criação, mas, quando informado, deve ser maior que zero.

---

# Autenticação

Google OAuth 2.0 (Authorization Code + PKCE) + JWT Bearer interno.

O handshake OAuth é feito à mão (HttpClient) sem usar Microsoft.AspNetCore.Authentication.Google.

A validação do ID Token do Google usa apenas Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync (JWKS).

Auto-registro: o primeiro login de uma conta Google desconhecida cria um Usuario. Papel Admin é atribuído no momento da criação se o e-mail estiver em Auth:AdminEmails.

Não implementar:

* Recuperação de senha (não há senhas)
* Cadastro manual de usuário
* Endpoint de refresh token

Endpoints públicos (sem [Authorize]):

* GET /api/v1/auth/google/challenge
* GET /api/v1/auth/google/callback

Todos os demais exigem Bearer token válido.

# Autorização

Papéis: Usuario (padrão) e Admin.

Handlers recebem ICurrentUser (nunca HttpContext direto).

Todas as queries e commands de Agendamento são escopados por UsuarioId. Exceção: Admin lê agendamentos de todos os Usuarios (mas não escreve).

Handlers de update/cancel/complete/delete que encontram Agendamento de outro Usuario retornam NotFound, não Forbidden (não vazar existência).

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
