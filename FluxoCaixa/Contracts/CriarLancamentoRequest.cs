using Lancamentos.Api.Domain.Enums;

namespace Lancamentos.Api.Contracts;

public class CriarLancamentoRequest
{
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public TipoLancamento Tipo { get; set; }
    public string? Descricao { get; set; }
}
