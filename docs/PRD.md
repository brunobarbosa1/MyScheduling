# PRD - Sistema de Agendamento para Salão de Beleza

## 1. Visão Geral

Sistema web para gerenciamento de agendamentos de uma profissional autônoma.

O objetivo principal é substituir a agenda física utilizada atualmente, centralizando os agendamentos em uma aplicação simples, rápida e intuitiva.

O sistema será utilizado inicialmente por uma única profissional.

Não existe suporte para múltiplos usuários ou múltiplos estabelecimentos nesta versão.

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

* Login de administrador.
* Autenticação via JWT.
* Cadastro de agendamentos.
* Consulta de agendamentos.
* Atualização de agendamentos.
* Cancelamento de agendamentos.
* Conclusão de agendamentos.
* Exclusão de agendamentos.
* Visualização da agenda.
* Controle do valor dos serviços.

## Não Incluído

* Cadastro de clientes.
* Cadastro de profissionais.
* Agendamento pelo cliente.
* Integração com WhatsApp.
* Integração com Google Agenda.
* Notificações.
* Relatórios.
* Dashboard financeiro.
* Multiusuário.
* Multiempresa.
* Aplicativo mobile.

---

# 4. Usuário do Sistema

## Administrador

Responsável pelo salão.

Possui acesso total ao sistema.

Será o único usuário da aplicação nesta versão.

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

O sistema possui apenas um perfil de acesso.

## Administrador

Permissões:

* Criar agendamentos
* Atualizar agendamentos
* Cancelar agendamentos
* Concluir agendamentos
* Excluir agendamentos
* Consultar agenda

Todos os endpoints exigem autenticação, exceto login.

---

# 8. Regras de Negócio

### RN001

Não permitir horários sobrepostos.

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
