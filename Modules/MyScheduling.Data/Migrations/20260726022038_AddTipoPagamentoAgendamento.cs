using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyScheduling.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoPagamentoAgendamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoPagamento",
                table: "Agendamentos",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoPagamento",
                table: "Agendamentos");
        }
    }
}
