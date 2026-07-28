using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyScheduling.Domain.Entities.Agendamentos;

namespace MyScheduling.Data.Mappings.Agendamentos;

public class AgendamentoMap : IEntityTypeConfiguration<Agendamento>
{
    public void Configure(EntityTypeBuilder<Agendamento> builder)
    {
        builder.ToTable("Agendamentos");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.ClienteNome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.ClienteTelefone)
            .HasMaxLength(20);

        builder.Property(a => a.Servico)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.ValorServico)
            .HasColumnType("numeric(10,2)");

        builder.Property(a => a.DataHoraInicio)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(a => a.DataHoraFim)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(a => a.TipoPagamento)
            .HasConversion<int?>();

        builder.Property(a => a.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(a => a.Observacao)
            .HasMaxLength(500);

        builder.Property(a => a.CriadoEm)
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(a => a.AtualizadoEm)
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(a => new { a.DataHoraInicio, a.DataHoraFim });
    }
}
