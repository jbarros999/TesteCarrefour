namespace Lancamentos.Api.Infra.Events;

public class LancamentoRegistradoEvent
{
    public Guid Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public string Tipo { get; set; } = "";
}
