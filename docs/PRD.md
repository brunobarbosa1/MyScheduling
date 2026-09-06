# PRD - Sistema de Agendamento para Salão de Beleza

## 1. Visão Geral

Sistema web para gerenciamento de agendamentos de profissionais autônomas.

O objetivo principal é substituir a agenda física, centralizando os agendamentos em uma aplicação simples, rápida e intuitiva.

O sistema suporta múltiplas profissionais, cada uma com sua agenda completamente isolada das demais.

Existe um papel de Administrador com visão global de leitura sobre todas as agendas do sistema.

---

# 2. Objetivo do Produto

Permitir que a profissional:

* Cadastre novos agendamentos.
* Consulte sua agenda.
* Atualize informações de um agendamento.
* Conclua atendimentos realizados.
* Cancele atendimentos.
* Exclua registros incorretos.
* Controle o valor dos serviços realizados.

---

# 3. Escopo do MVP

## Incluído

* Autenticação via Google OAuth 2.0 (Authorization Code + PKCE).
* Sessão via JWT emitido pelo backend.
* Auto-registro de profissional no primeiro login com Google.
* Papel de Administrador com leitura global das agendas.
* Isolamento de dados por profissional.
* Cadastro de agendamentos.
* Consulta de agendamentos.
* Atualização de agendamentos.
* Cancelamento de agendamentos.
* Conclusão de agendamentos.
* Exclusão de agendamentos.
* Visualização da agenda.
* Controle do valor dos serviços.

## Não Incluído

* Autenticação por senha.
* Convite explícito de usuários por admin.
* Cadastro de clientes.
* Agendamento pelo cliente final.
* Integração com WhatsApp.
* Integração com Google Agenda.
* Notificações.
* Relatórios.
* Dashboard financeiro.
* Multiempresa (organizações agrupando usuários).
* Aplicativo mobile nativo.
* Auto-desativação ou exclusão de conta pela profissional.

---

# 4. Usuários do Sistema

O sistema possui dois papéis, ambos autenticados via Google.

## Usuario (padrão)

Profissional que gerencia a própria agenda.

Permissões: CRUD completo apenas sobre os agendamentos do próprio Usuario.

## Admin

Profissional com privilégio adicional de leitura global.

Permissões: as mesmas do Usuario + leitura de agendamentos de qualquer Usuario.

Não pode criar, atualizar ou excluir agendamentos de outros Usuarios.

O papel Admin é atribuído automaticamente no primeiro login se o e-mail da conta Google constar na whitelist de administradores da configuração (Auth:AdminEmails).

---

# 5. Entidade Principal

## Agendamento

Representa um horário reservado para atendimento.

Campos previstos:

* ClienteNome
* ClienteTelefone
* Servico
* ValorServico
* DataHoraInicio
* DataHoraFim
* Status
* Observacao

---

# 6. Status

## Agendado

Atendimento aguardando execução.

## Concluido

Atendimento realizado.

## Cancelado

Atendimento cancelado.

---

# 7. Autenticação

Autenticação exclusivamente via Google OAuth 2.0 usando o fluxo Authorization Code com PKCE.

Após validação bem-sucedida do login, o backend emite um JWT interno usado como Bearer token nas requisições subsequentes.

## Auto-registro

O primeiro login de uma conta Google desconhecida cria automaticamente um novo Usuario.

Se o e-mail estiver na whitelist de administradores (Auth:AdminEmails) no momento da criação, o Usuario recebe papel Admin. Caso contrário, papel Usuario.

## Endpoints públicos

* GET /api/v1/auth/google/challenge — inicia o fluxo (redirect para o Google)
* GET /api/v1/auth/google/callback — callback do Google (troca code por tokens e emite JWT)

Todos os demais endpoints exigem Bearer token válido.

---

# 8. Regras de Negócio

### RN001

Não permitir horários sobrepostos dentro da agenda do mesmo Usuario. Agendamentos de Usuarios distintos não conflitam entre si.

### RN002

DataHoraFim deve ser maior que DataHoraInicio.

### RN003

Não permitir criação de horários no passado.

### RN004

Agendamentos cancelados não bloqueiam horários.

### RN005

ClienteNome é obrigatório.

### RN006

ValorServico é opcional e pode ser definido depois da criação, mas, quando informado, deve ser maior que zero.

---

# 9. Casos de Uso

## Autenticar via Google

Iniciar o fluxo OAuth, retornar do consentimento com código de autorização, receber JWT interno para uso nas próximas requisições.

## Criar Agendamento

Cadastrar novo horário.

## Atualizar Agendamento

Alterar informações do horário.

## Obter Agendamento

Consultar um agendamento.

## Listar Agendamentos

Consultar agenda.

## Cancelar Agendamento

Alterar status para Cancelado.

## Concluir Agendamento

Alterar status para Concluido.

## Excluir Agendamento

Remover registro.

---

# 10. Critério de Sucesso

O sistema será considerado pronto quando a profissional conseguir abandonar completamente a agenda física para controle diário dos atendimentos.
