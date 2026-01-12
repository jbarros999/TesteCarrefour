
using Lancamentos.Api.Domain.Enums;

namespace Lancamentos.Api.Domain.Entities
{
    public class Lancamento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public TipoLancamento Tipo { get; set; }
        public string? Descricao { get; set; }
    }
}
