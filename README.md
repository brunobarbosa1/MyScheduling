# MyScheduling

API de agendamentos para uma profissional autônoma de salão de beleza. O objetivo é substituir a agenda física usada hoje e, no caminho, exercitar boas práticas de arquitetura no ecossistema .NET.

Escopo da V1: um único usuário administrador gerenciando agendamentos (criar, atualizar, cancelar, concluir, excluir e consultar).

---

## Stack

| Camada | Tecnologia |
| --- | --- |
| Runtime | .NET 10 (`net10.0`) |
| API | ASP.NET Core Web API (Controllers) |
| Persistência | Entity Framework Core 10 + Npgsql |
| Banco | PostgreSQL 16 |
| Validação | FluentValidation 12 |
| Documentação | Swagger / Swashbuckle |
| Testes | xUnit |
| Infra local | Docker Compose |

---

## Pré-requisitos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) (o projeto tem como alvo `net10.0`)
- [Docker](https://docs.docker.com/get-docker/) com Docker Compose
- Opcional: `dotnet-ef` para trabalhar com migrations
  ```bash
  dotnet tool install --global dotnet-ef
  ```

---

## Rodando localmente

O setup local é híbrido de propósito: **o Postgres roda em container, a API roda direto na sua máquina**. Isso mantém o ciclo de edição/debug rápido, sem rebuild de imagem a cada alteração.

### 1. Suba o banco

```bash
docker compose up -d
```

Isso levanta apenas o serviço `db` (`postgres:16-alpine`, container `myscheduling-db-local`) na porta `5432`, com volume nomeado `postgres_data_local` para os dados persistirem entre restarts.

Todos os valores têm default de desenvolvimento, então **nenhum arquivo `.env` é necessário**. Se quiser sobrescrever algo, crie um `.env` na raiz:

| Variável | Default |
| --- | --- |
| `POSTGRES_USER` | `myscheduling` |
| `POSTGRES_PASSWORD` | `postgres` |
| `POSTGRES_DB` | `myscheduling` |
| `POSTGRES_PORT` | `5432` |

> Se mudar qualquer um desses, ajuste também a connection string em `UI/MyScheduling.API/appsettings.Development.json`.

### 2. Crie o `appsettings.Development.json`

Esse arquivo não é versionado. Copie o exemplo:

```bash
cp UI/MyScheduling.API/appsettings.Development.example.json \
   UI/MyScheduling.API/appsettings.Development.json
```

O exemplo já vem com a connection string que casa com os defaults do compose, então não é preciso editar nada para o setup padrão.

### 3. Rode a API

```bash
dotnet run --project UI/MyScheduling.API
```

Não é preciso rodar migrations manualmente: a API aplica as migrations pendentes no startup (`MigrateDatabase()` em `Program.cs:30`).

A API sobe em:

- HTTP — http://localhost:5167
- HTTPS — https://localhost:7096
- Swagger UI — http://localhost:5167/swagger *(exposto apenas em `Development`)*

Em outros ambientes a connection string vem da variável `ConnectionStrings__Postgres`. Se nem ela nem o `appsettings.Development.json` estiverem definidos, a aplicação falha no startup com mensagem explícita — é intencional, para não subir apontando para lugar nenhum.

### 4. Derrubando

```bash
docker compose down          # para o banco, mantém os dados
docker compose down -v       # para o banco e apaga o volume
```

### Testando os endpoints

O arquivo `UI/MyScheduling.API/MyScheduling.API.http` tem requisições prontas para todos os endpoints (Rider, VS Code + REST Client, Visual Studio). Ajuste a variável `@MyScheduling.API_HostAddress` para `http://localhost:5167` ao rodar via `dotnet run`.

---

## Testes

```bash
dotnet test MyScheduling.slnx
```

São testes de unidade puros — sem banco, sem container, sem framework de mock. As dependências são substituídas por fakes escritos à mão (`FakeAgendamentoRepository`, `FixedTimeProvider`), e o foco está nas regras de negócio dos command handlers: sobreposição de horário, agendamento no passado, validação de campos e transições de status.

---

## CI

O workflow em `.github/workflows/ci.yml` roda em push e pull request na `main`, em dois jobs encadeados:

1. **Build e testes** — `restore` → `build -c Release` → `dotnet test`, com cache de pacotes NuGet. O `.trx` fica disponível como artefato da execução.
2. **Imagem Docker** — depende do job anterior, então nada é publicado sem os testes passarem. Em pull request a imagem é apenas construída (valida o `Dockerfile`); em push na `main` ela é publicada no GitHub Container Registry.

O job usa o SDK **10.0.x**, que já entende o `MyScheduling.slnx` e traz o runtime `net10.0` necessário para executar os testes.

### Imagem publicada

```
ghcr.io/brunobarbosa1/myscheduling:latest
ghcr.io/brunobarbosa1/myscheduling:sha-<commit>
```

A autenticação usa o `GITHUB_TOKEN` do próprio workflow — não é preciso criar nenhum secret. O pacote nasce **privado**; para consumi-lo de fora do GitHub Actions é necessário ou torná-lo público nas configurações do pacote, ou fazer `docker login ghcr.io` com um Personal Access Token de escopo `read:packages`.

### O que ainda não existe

Não há job de deploy. Ele entra quando a VPS existir e puder ser validado de verdade — encaixa como um job novo com `needs: image`. Nesse momento, o `docker-compose.prod.yml` deixa de usar `build:` e passa a usar `image: ghcr.io/brunobarbosa1/myscheduling:latest` (é o `TODO(CI/CD)` anotado no arquivo).

---

## Migrations

As migrations vivem em `Modules/MyScheduling.Data/Migrations/`. Como o `DbContext` está em um projeto e o host em outro, os comandos precisam de `-p` (projeto das migrations) e `-s` (projeto de startup):

```bash
# criar
dotnet ef migrations add NomeDaMigration -p Modules/MyScheduling.Data -s UI/MyScheduling.API

# listar
dotnet ef migrations list -p Modules/MyScheduling.Data -s UI/MyScheduling.API

# aplicar manualmente (normalmente desnecessário — o startup já aplica)
dotnet ef database update -p Modules/MyScheduling.Data -s UI/MyScheduling.API
```

Para gerar migrations não é preciso ter o banco no ar: `AppDbContextFactory` fornece uma connection string de design-time, lida de `ConnectionStrings__Postgres` ou caindo no default local.

---

## Estrutura do projeto

```
MyScheduling.slnx
│
├── Core/
│   ├── MyScheduling.Common/              # Result Pattern (Result, Result<T>, Error, ErrorType)
│   └── MyScheduling.DependencyInjection/ # Registro explícito de DI, por módulo
│
├── Modules/
│   ├── MyScheduling.Domain/              # Entidades, enums e contratos de repositório
│   ├── MyScheduling.Data/                # DbContext, mappings, repositórios, migrations
│   ├── MyScheduling.Application/         # Commands, Queries, Handlers, Filters, Validators
│   └── MyScheduling.Presentations/       # ViewModels (contratos de saída da API)
│
├── UI/
│   └── MyScheduling.API/                 # Host ASP.NET Core: Program.cs e Controllers
│
├── tests/
│   └── MyScheduling.Tests/               # xUnit
│
└── docs/                                 # PRD.md e SDD.md
```

Dentro de cada projeto, as pastas são organizadas **por módulo de domínio** (`Agendamentos/`, `Usuarios/`), e não por tipo técnico. Ao adicionar um novo domínio, ele ganha uma subpasta em cada camada.

### O papel de cada camada

**`MyScheduling.Common`** — não referencia nada. Contém o Result Pattern: `Result`, `Result<T>`, `Error` e `ErrorType` (`Failure`, `Validation`, `NotFound`, `Conflict`). É o vocabulário de erro que atravessa todas as camadas.

**`MyScheduling.Domain`** — o núcleo, sem dependências de framework. Entidades com setters privados e comportamento explícito: `Agendamento` só muda de estado por `Atualizar()`, `Cancelar()` e `Concluir()`, e só nasce por `Agendamento.Factory.CriarNovo()`. A base `Entity` cuida de `Id`, `CriadoEm` e `AtualizadoEm`. Aqui também moram as *interfaces* de repositório — a implementação fica na Data (dependência invertida).

**`MyScheduling.Data`** — EF Core. `AppDbContext`, configurações via `IEntityTypeConfiguration` em `Mappings/`, repositórios concretos e as migrations. Não há repositório genérico: `IAgendamentoRepository` expõe exatamente o que a aplicação usa, incluindo `Query()` (`IQueryable`) para as queries projetarem em cima e `ExisteSobreposicaoAsync()` para a regra de conflito de horário.

**`MyScheduling.Application`** — CQRS manual, sem MediatR:

- `Commands/` — records de entrada que carregam o próprio `Validator` (classe aninhada FluentValidation) exposto por `Validate()`.
- `CommandHandlers/` — um handler por caso de uso, cada um com sua interface (`ICriarAgendamentoCommandHandler` etc.), que é o que a DI e o controller enxergam.
- `Filters/` — records de entrada das leituras (o equivalente da query ao command).
- `Queries/` — leitura pura: `AsNoTracking()` + projeção via `Expression` (`AgendamentoProjections.ToViewModel`), traduzida para SQL pelo EF sem materializar a entidade.

**`MyScheduling.Presentations`** — ViewModels. `AgendamentoViewModel` tem um `implicit operator` a partir da entidade, usado pelos commands; as queries usam a `Expression` equivalente. Queries nunca retornam entidades.

**`MyScheduling.DependencyInjection`** — todo o registro de serviços, agrupado por responsabilidade (`Commands/`, `Queries/`, `Persistence/`, `Services/`) e composto em extension methods encadeáveis. Nada de assembly scanning: cada handler é registrado à mão.

**`MyScheduling.API`** — controllers finos. `ApiControllerBase.HandleFailure()` é o único ponto que traduz `ErrorType` em status HTTP, devolvendo `ProblemDetails`. Nenhuma regra de negócio aqui.

### Fluxo de uma requisição

```
Controller
   → Command/Filter (record de entrada)
      → Handler (valida, aplica regra, orquestra)
         → Repository / DbContext
      ← Result<T>
   ← ViewModel  ou  ProblemDetails (via HandleFailure)
```

Regras de negócio não lançam exceção: elas retornam `Result.Failure(Error.…)`. Exceções ficam reservadas para falhas inesperadas ou de infraestrutura. O mapeamento é direto:

| `ErrorType` | HTTP |
| --- | --- |
| `Validation` | 400 |
| `NotFound` | 404 |
| `Conflict` | 409 |
| `Failure` | 500 |

---

## API

Base: `/api/v1/agendamentos`

| Método | Rota | Descrição |
| --- | --- | --- |
| `GET` | `/api/v1/agendamentos?status=` | Lista agendamentos; `status` opcional (`Agendado`, `Concluido`, `Cancelado`) |
| `GET` | `/api/v1/agendamentos/data/{data}` | Lista os agendamentos de um dia (`yyyy-MM-dd`) |
| `GET` | `/api/v1/agendamentos/{id}` | Obtém um agendamento por id |
| `POST` | `/api/v1/agendamentos` | Cria um agendamento |
| `PUT` | `/api/v1/agendamentos/{id}` | Atualiza um agendamento |
| `PATCH` | `/api/v1/agendamentos/{id}/cancelar` | Cancela |
| `PATCH` | `/api/v1/agendamentos/{id}/concluir` | Conclui |
| `DELETE` | `/api/v1/agendamentos/{id}` | Exclui |

Enums trafegam como **string** no JSON (`JsonStringEnumConverter`), não como número.

### Exemplo

```http
POST /api/v1/agendamentos
Content-Type: application/json

{
  "clienteNome": "Maria Silva",
  "clienteTelefone": "11999998888",
  "servico": "Corte + Escova",
  "valorServico": 120.00,
  "dataHoraInicio": "2026-07-27T14:00:00-03:00",
  "dataHoraFim": "2026-07-27T15:30:00-03:00",
  "tipoPagamento": "PIX",
  "observacao": "Cliente prefere atendimento sem secador."
}
```

---

## Domínio

### Agendamento

| Campo | Tipo | Obrigatório |
| --- | --- | --- |
| `ClienteNome` | `string` (máx. 200) | sim |
| `ClienteTelefone` | `string?` (máx. 20) | não |
| `Servico` | `string` (máx. 200) | sim |
| `ValorServico` | `decimal?` | não |
| `DataHoraInicio` | `DateTimeOffset` | sim |
| `DataHoraFim` | `DateTimeOffset` | sim |
| `TipoPagamento` | `PIX` \| `CREDITO` \| `DEBITO` | não |
| `Status` | `Agendado` \| `Concluido` \| `Cancelado` | definido pelo sistema |
| `Observacao` | `string?` (máx. 500) | não |


## Documentação adicional

- `docs/PRD.md` — requisitos do produto
- `docs/SDD.md` — desenho da solução
- `CLAUDE.md` — convenções e restrições de arquitetura para agentes de IA
