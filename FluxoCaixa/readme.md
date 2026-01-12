# Lancamentos.Api

## Lançamentos API – Registro de Débitos e Créditos

Este serviço é responsável pelo registro de lançamentos financeiros (débitos e créditos) do fluxo de caixa do comerciante.
Ele é o **system of record** do domínio de lançamentos e é a fonte de eventos para outros serviços, como o consolidador diário.

## Capacidades de negócio

- Registrar lançamento financeiro
- Listar lançamentos realizados
- Publicar eventos de domínio após o registro

## Arquitetura

- Arquitetura orientada a eventos
- Domínio de lançamentos independente
- Publicação de eventos após a gravação
- Armazenamento próprio (InMemory neste teste)
- Desacoplamento em relação ao serviço de consolidação

Não existe consulta direta ao consolidador.
A comunicação entre serviços ocorre por eventos.

## Como executar o projeto

Na raiz da solução:

dotnet run --project Lancamentos.Api

Swagger (em ambiente Development):

https://localhost:5001/swagger

A porta pode variar conforme a configuração local.

## Endpoints principais

### Registrar lançamento

POST /lancamentos

Exemplo de corpo:

{
  "data": "2026-01-08",
  "valor": 100,
  "tipo": "Credito",
  "descricao": "Venda do dia"
}

### Listar lançamentos

GET /lancamentos

## Eventos publicados

Após o registro de um lançamento é publicado o evento:

LancamentoRegistradoEvent

Campos principais:

- Id
- Data
- Valor
- Tipo

Esse evento é consumido pelo serviço Consolidado.Api.

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- Swagger / OpenAPI
- Repositório InMemory
- Arquitetura orientada a eventos

## Decisões arquiteturais relevantes

- O serviço de lançamentos não depende do consolidador
- Permite consistência eventual
- Atende ao requisito do desafio:

O serviço de controle de lançamento não deve ficar indisponível se o sistema de consolidado diário cair
