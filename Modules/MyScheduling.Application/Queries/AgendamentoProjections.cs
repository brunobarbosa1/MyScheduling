using System.Linq.Expressions;
using MyScheduling.Domain.Entities;
using MyScheduling.Presentations.Agendamentos;

namespace MyScheduling.Application.Queries;

internal static class AgendamentoProjections
{
    /// <summary>
    /// Projeção de leitura (Entidade -> ViewModel) usada nas queries.
    /// Como é uma Expression, é traduzida para SQL pelo EF (sem materializar a entidade).
    /// </summary>
    public static readonly Expression<Func<Agendamento, AgendamentoViewModel>> ToViewModel =
        a => new AgendamentoViewModel
        {
            Id = a.Id,
            ClienteNome = a.ClienteNome,
            ClienteTelefone = a.ClienteTelefone,
            Servico = a.Servico,
            ValorServico = a.ValorServico,
            DataHoraInicio = a.DataHoraInicio,
            DataHoraFim = a.DataHoraFim,
            Status = a.Status,
            Observacao = a.Observacao,
            CriadoEm = a.CriadoEm,
            AtualizadoEm = a.AtualizadoEm
        };
}
