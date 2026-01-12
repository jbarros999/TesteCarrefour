using Consolidado.Api.Data;
using Consolidado.Api.Models;
using Consolidado.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core InMemory
builder.Services.AddDbContext<ConsolidadoContext>(opt =>
    opt.UseInMemoryDatabase("ConsolidadoDB"));

// regras de negócio
builder.Services.AddScoped<ConsolidadorService>();

// API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger somente em Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//
// 🔹 1) Endpoint que SIMULA o recebimento do evento
//     (equivalente ao bus.PublishAsync do Lançamentos)
//
app.MapPost("/eventos/lancamento-registrado", async (
    LancamentoRegistradoEvent evt,
    ConsolidadorService service) =>
{
    await service.ProcessarAsync(evt);
    return Results.Accepted();
});

//
// 🔹 2) Consulta do consolidado diário
//
app.MapGet("/consolidado/{data}", async (string data, ConsolidadoContext db) =>
{
    var parsed = DateOnly.Parse(data);

    var result = await db.Consolidados
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.DataReferencia == parsed);

    return result is null ? Results.NotFound() : Results.Ok(result);
});

app.Run();
