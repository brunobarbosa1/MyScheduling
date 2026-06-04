using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyScheduling.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteNome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ClienteTelefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Servico = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ValorServico = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DataHoraInicio = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DataHoraFim = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_DataHoraInicio_DataHoraFim",
                table: "Agendamentos",
                columns: new[] { "DataHoraInicio", "DataHoraFim" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendamentos");
        }
    }
}
