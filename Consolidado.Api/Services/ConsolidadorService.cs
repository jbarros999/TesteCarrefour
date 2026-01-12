using Consolidado.Api.Data;
using Consolidado.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Consolidado.Api.Services;

public class ConsolidadorService
{
    private readonly ConsolidadoContext _context;

    public ConsolidadorService(ConsolidadoContext context)
    {
        _context = context;
    }

    public async Task ProcessarAsync(LancamentoRegistradoEvent evt)
    {
        // idempotência
        if (await _context.EventosProcessados.AnyAsync(e => e.Id == evt.EventId))
            return;

        var agregado = await _context.Consolidados
            .FirstOrDefaultAsync(x => x.DataReferencia == evt.Data);

        if (agregado is null)
        {
            agregado = new ConsolidadoDiario
            {
                Id = Guid.NewGuid(),
                DataReferencia = evt.Data,
                UltimaAtualizacao = DateTime.UtcNow
            };

            _context.Consolidados.Add(agregado);
        }

        if (evt.Tipo.Equals("Credito", StringComparison.OrdinalIgnoreCase))
            agregado.TotalCreditos += evt.Valor;
        else
            agregado.TotalDebitos += evt.Valor;

        agregado.UltimaAtualizacao = DateTime.UtcNow;

        _context.EventosProcessados.Add(new EventoProcessado
        {
            Id = evt.EventId,
            ProcessadoEm = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
}
