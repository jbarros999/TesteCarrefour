using Lancamentos.Api.Contracts;
using Lancamentos.Api.Domain.Entities;
using Lancamentos.Api.Infra.Events;
using Lancamentos.Api.Repositories;
using Lancamentos.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddSingleton<LancamentoRepository>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/lancamentos", async (CriarLancamentoRequest req,
                                   LancamentoRepository repo,
                                   IEventBus bus) =>
{
    var (valido, erro) = LancamentoValidatorService.Validar(req);

    if (!valido)
        return Results.BadRequest(new { erro });

    var lancamento = new Lancamento
    {
        Data = req.Data,
        Valor = req.Valor,
        Tipo = req.Tipo,
        Descricao = req.Descricao
    };

    repo.Adicionar(lancamento);

    // publica evento
    var evt = new LancamentoRegistradoEvent
    {
        Id = lancamento.Id,
        Data = lancamento.Data,
        Valor = lancamento.Valor,
        Tipo = lancamento.Tipo.ToString()
    };

    await bus.PublishAsync(evt);

    return Results.Created($"/lancamentos/{lancamento.Id}", lancamento);
});

app.MapGet("/lancamentos", (LancamentoRepository repo) =>
{
    return Results.Ok(repo.Listar());
});

app.Run();
