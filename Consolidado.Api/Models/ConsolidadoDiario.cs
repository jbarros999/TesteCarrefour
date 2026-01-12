namespace Consolidado.Api.Models;

public class ConsolidadoDiario
{
    public Guid Id { get; set; }
    public DateOnly DataReferencia { get; set; }

    public decimal TotalCreditos { get; set; }
    public decimal TotalDebitos { get; set; }

    public decimal SaldoFinal => TotalCreditos - TotalDebitos;

    public DateTime UltimaAtualizacao { get; set; }
}
