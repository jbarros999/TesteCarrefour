namespace Consolidado.Api.Models;

public class LancamentoRegistradoEvent
{
    public Guid EventId { get; set; }
    public Guid LancamentoId { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = default!;
    public DateOnly Data { get; set; }
}
