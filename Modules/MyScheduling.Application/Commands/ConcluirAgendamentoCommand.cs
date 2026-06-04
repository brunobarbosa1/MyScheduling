using FluentValidation;
using FluentValidation.Results;

namespace MyScheduling.Application.Agendamentos.Commands;

public sealed record ConcluirAgendamentoCommand
{
    public Guid Id { get; init; }

    public ValidationResult Validate() => new Validator().Validate(this);

    internal sealed class Validator : AbstractValidator<ConcluirAgendamentoCommand>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O identificador do agendamento é obrigatório.");
        }
    }
}
