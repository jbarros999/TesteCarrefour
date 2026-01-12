using Consolidado.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Consolidado.Api.Data;

public class ConsolidadoContext : DbContext
{
    public ConsolidadoContext(DbContextOptions<ConsolidadoContext> options) : base(options) { }

    public DbSet<ConsolidadoDiario> Consolidados => Set<ConsolidadoDiario>();
    public DbSet<EventoProcessado> EventosProcessados => Set<EventoProcessado>();
}
