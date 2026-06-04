using FluentValidation;
using FluentValidation.Results;

namespace MyScheduling.Application.Commands;

public sealed record AtualizarAgendamentoCommand
{
    public Guid Id { get; init; }
    public string ClienteNome { get; init; } = string.Empty;
    public string? ClienteTelefone { get; init; }
    public string Servico { get; init; } = string.Empty;
    public decimal ValorServico { get; init; }
    public DateTimeOffset DataHoraInicio { get; init; }
    public DateTimeOffset DataHoraFim { get; init; }
    public string? Observacao { get; init; }

    public ValidationResult Validate() => new Validator().Validate(this);

    internal sealed class Validator : AbstractValidator<AtualizarAgendamentoCommand>
    {
        public Validator()
        {
            RuleFor(c => c.Id)
                .NotEmpty().WithMessage("O identificador do agendamento é obrigatório.");

            RuleFor(c => c.ClienteNome)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                .MaximumLength(200).WithMessage("O nome do cliente deve ter no máximo 200 caracteres.");

            RuleFor(c => c.ClienteTelefone)
                .MaximumLength(20).WithMessage("O telefone deve ter no máximo 20 caracteres.");

            RuleFor(c => c.Servico)
                .NotEmpty().WithMessage("O serviço é obrigatório.")
                .MaximumLength(200).WithMessage("O serviço deve ter no máximo 200 caracteres.");

            RuleFor(c => c.ValorServico)
                .GreaterThan(0).WithMessage("O valor do serviço deve ser maior que zero.");

            RuleFor(c => c.DataHoraFim)
                .GreaterThan(c => c.DataHoraInicio)
                .WithMessage("A data/hora final deve ser maior que a inicial.");

            RuleFor(c => c.Observacao)
                .MaximumLength(500).WithMessage("A observação deve ter no máximo 500 caracteres.");
        }
    }
}
