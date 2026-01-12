# Consolidado.Api

## Consolidado Diário – Saldo Consolidado por Dia

Este serviço é responsável por consolidar lançamentos financeiros por dia, consumindo eventos produzidos pelo serviço de lançamentos.
Ele mantém um armazenamento próprio otimizado para leitura e permite consultar o saldo consolidado diário.

## Capacidades de negócio

- Consolidar eventos de lançamentos
- Manter agregação diária de créditos e débitos
- Fornecer consulta de saldo consolidado por data

## Arquitetura

- Arquitetura orientada a eventos
- Banco separado do domínio de lançamentos
- Consistência eventual
- Idempotência baseada em eventId
- Escalabilidade independente por serviço

Requisitos do desafio atendidos:

- O consolidador pode falhar sem impactar o serviço de lançamentos
- Em picos de carga é possível escalar horizontalmente
- Serviços desacoplados por troca de eventos

## Como executar o projeto

Na raiz da solução:

dotnet run --project Consolidado.Api

Swagger (em ambiente Development):

https://localhost:5002/swagger

A porta pode variar conforme o ambiente.

## Endpoints

### Recebimento de evento de lançamento (simulação de mensageria)

POST /eventos/lancamento-registrado

Exemplo de corpo:

{
  "eventId": "aaaaaaaa-bbbb-cccc-dddd-111111111111",
  "lancamentoId": "bbbbbbbb-aaaa-cccc-dddd-222222222222",
  "valor": 100,
  "tipo": "Credito",
  "data": "2026-01-08"
}

Observação:
Em ambiente real o evento seria recebido via mensageria (Kafka, RabbitMQ, etc.).
No teste técnico ele é simulado por HTTP para simplificação.

### Consulta do consolidado diário

GET /consolidado/{data}

Exemplo:

GET /consolidado/2026-01-08

Resposta de exemplo:

{
  "dataReferencia": "2026-01-08",
  "totalCreditos": 150,
  "totalDebitos": 40,
  "saldoFinal": 110
}

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core InMemory
- Swagger / OpenAPI
- Processamento idempotente
- Arquitetura orientada a eventos

## Requisitos não funcionais atendidos

- Independência entre serviços
- Tolerância a falhas
- Consistência eventual
- Possibilidade de perda máxima tolerável em picos, conforme enunciado
- Escalabilidade horizontal do serviço de consolidação

## Possíveis evoluções

- Mensageria real (Kafka, RabbitMQ, etc.)
- Banco analítico dedicado
- Cache distribuído para consultas frequentes
- Observabilidade e métricas
- Reprocessamento automático a partir de DLQ
