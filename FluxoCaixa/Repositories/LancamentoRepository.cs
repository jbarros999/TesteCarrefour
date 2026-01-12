using System.Collections.Concurrent;
using Lancamentos.Api.Domain.Entities;

namespace Lancamentos.Api.Repositories;

public class LancamentoRepository
{
    private readonly ConcurrentBag<Lancamento> _lancamentos = new();

    public void Adicionar(Lancamento lancamento)
    {
        _lancamentos.Add(lancamento);
    }

    public IEnumerable<Lancamento> Listar()
    {
        return _lancamentos.ToArray();
    }
}
