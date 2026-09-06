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

Representa uma profissional cadastrada no sistema.

Campos:

* Id
* Nome
* Email
* GoogleSub (identificador único e imutável do Google)
* Papel (Usuario | Admin)
* CriadoEm
* AtualizadoEm

## PapelUsuario

* Usuario
* Admin

---

## Agendamento

Campos:

* Id
* UsuarioId (FK para Usuario — dono da agenda)
* ClienteNome
* ClienteTelefone
* Servico
* ValorServico
* DataHoraInicio
* DataHoraFim
* Status
* TipoPagamento
* Observacao
* CriadoEm
* AtualizadoEm

Cada Agendamento pertence a exatamente um Usuario. Usuarios distintos não compartilham agendamentos.

---

# StatusAgendamento

* Agendado
* Concluido
* Cancelado

---

# Commands

## Auth

* IniciarLoginGoogle (gera state + PKCE, monta URL de autorização do Google)
* CompletarLoginGoogle (valida state, troca code por tokens, valida ID Token, cria/localiza Usuario, emite JWT)

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

Google OAuth 2.0 (Authorization Code + PKCE) + JWT Bearer interno emitido pelo backend.

## Fluxo

1. Cliente chama GET /api/v1/auth/google/challenge
2. Backend gera state (anti-CSRF) e par PKCE (code_verifier + code_challenge), persiste em cache com expiração curta
3. Backend redireciona para authorization endpoint do Google
4. Usuário autentica e consente
5. Google redireciona para GET /api/v1/auth/google/callback?code=...&state=...
6. Backend valida state, recupera code_verifier
7. Backend troca code + code_verifier por tokens no endpoint do Google (server-to-server)
8. Backend valida assinatura, issuer e audience do ID Token
9. Backend cria (primeiro login) ou localiza Usuario pelo GoogleSub
10. Backend emite JWT próprio com claims sub, email, name, role
11. Cliente usa esse JWT como Bearer nos endpoints protegidos

## Bibliotecas

* HttpClient tipado para chamadas ao Google
* Google.Apis.Auth (apenas GoogleJsonWebSignature.ValidateAsync para validação do ID Token via JWKS)
* Microsoft.AspNetCore.Authentication.JwtBearer + Microsoft.IdentityModel.Tokens para emitir e validar o JWT interno

Não usamos Microsoft.AspNetCore.Authentication.Google (esconde detalhes do fluxo que queremos controlar explicitamente).

## Configuração

* Google:ClientId
* Google:ClientSecret
* Google:RedirectUri
* Google:Scopes (openid email profile)
* Jwt:Issuer, Jwt:Audience, Jwt:Key, Jwt:ExpirationSeconds
* Auth:AdminEmails (lista)

---

# Endpoints de Autenticação

GET /api/v1/auth/google/challenge → 302 para accounts.google.com

GET /api/v1/auth/google/callback?code=...&state=...

Response de sucesso:

{
  "accessToken": "jwt-token",
  "expiresIn": 3600
}

---

# Usuários

Não há usuário inicial pré-criado.

Auto-registro: o primeiro login de uma conta Google desconhecida cria automaticamente um Usuario.

Papel Admin é atribuído no momento da criação se o e-mail constar em Auth:AdminEmails.

---

# Autorização e Multi-tenancy

Todos os endpoints (exceto os dois de autenticação) exigem Bearer token válido via atributo [Authorize].

## Contexto do usuário atual

Interface ICurrentUser exposta em MyScheduling.Application resolve para a implementação em MyScheduling.API que lê as claims do HttpContext.

Handlers dependem apenas da interface, nunca do HttpContext.

## Isolamento

Toda operação sobre Agendamento é escopada por UsuarioId:

* Commands: associam o Agendamento ao Usuario atual na criação; verificam ownership antes de update, cancel, complete e delete
* Queries: filtram por UsuarioId == currentUser.Id

## Papel Admin

Exceção única ao filtro de ownership: quando currentUser.Papel == Admin, queries de listagem retornam agendamentos de todos os Usuarios.

Commands de escrita não têm exceção para Admin — Admin escreve apenas na própria agenda.

## Proteção contra IDOR

Handlers de update, cancel, complete e delete que encontram um agendamento cujo UsuarioId não corresponde ao usuário atual devem retornar NotFound (não Forbidden), para não vazar existência de recursos alheios.

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
