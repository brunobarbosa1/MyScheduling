using FluentValidation;
using FluentValidation.Results;

namespace MyScheduling.Application.Commands;

public sealed record CancelarAgendamentoCommand
{
    public Guid Id { get; init; }

    public ValidationResult Validate() => new Validator().Validate(this);

    internal sealed class Validator : AbstractValidator<CancelarAgendamentoCommand>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O identificador do agendamento é obrigatório.");
        }
    }
}
