# ADR-001 – Adoção de Arquitetura Orientada a Eventos

## Status
Aceito

## Contexto
O sistema precisa registrar lançamentos financeiros e gerar consolidação diária.
Existe requisito não funcional explícito informando que o serviço de lançamentos não deve ficar indisponível caso o serviço de consolidação falhe.
Há requisito de desempenho informando que o serviço de consolidação deve suportar picos de 50 requisições por segundo, admitindo no máximo 5% de perda, mantendo os lançamentos funcionais mesmo em caso de falha do consolidador.

Há também necessidade de:
- escalabilidade
- resiliência
- desacoplamento entre serviços
- picos de requisições no consolidador

## Decisão
Os lançamentos são a fonte de verdade (system of record) e publicam eventos de domínio em um broker de mensagens (ex.: Kafka/RabbitMQ). O serviço de consolidação consome esses eventos de forma assíncrona e idempotente, persistindo agregações diárias.
Foi adotada uma arquitetura orientada a eventos entre os domínios:

- serviço de lançamentos publica eventos de lançamentos registrados
- serviço de consolidação consome eventos de forma assíncrona
- mensageria atua como buffer e garante desacoplamento

A comunicação com clientes permanece REST.

## Alternativas Consideradas

### 1) Integração síncrona via REST entre serviços
Prós:
- implementação simples
- menor curva de aprendizado

Contras:
- forte acoplamento
- dependência direta entre serviços
- falha do consolidador impacta lançamentos

Motivo do descarte:
- viola requisito explícito de independência entre serviços

### 2) Banco único com job de consolidação
Prós:
- simplicidade operacional

Contras:
- acoplamento de domínios
- escala limitada
- alto risco de lock e contenção
- baixa flexibilidade evolutiva

## Consequências
Mensagens são produzidas utilizando o padrão Outbox, evitando perda entre a transação de gravação do lançamento e publicação do evento.
O consumidor implementa processamento idempotente para evitar duplicidade em cenários de retry.


Positivas:
- tolerância a falhas
- escalabilidade independente por domínio
- processamento assíncrono de alto volume

Negativas:
- aumento de complexidade operacional
- necessidade de mensageria
- monitoração mais sofisticada

## Riscos e Mitigações
Risco: perda de mensagens  
Mitigação: uso de DLQ e política de retry

Risco: consolidação com atraso  
Mitigação: SLAs e ajuste de escala automática

## Evidências
Arquiteturas orientadas a eventos são amplamente adotadas para desacoplamento entre domínios, resiliência a falhas e escalabilidade horizontal em cenários financeiros e de alta volumetria.

