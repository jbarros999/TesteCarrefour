# ADR-002 – Separação entre banco transacional e banco de consolidação

## Status
Aceito

## Contexto
O domínio de lançamentos exige:
- forte consistência
- operações ACID

O domínio de consolidação exige:
- leitura massiva
- agregações
- consultas analíticas

O serviço de consolidação deve suportar picos de 50 requisições por segundo, com até 5% de perda. Além disso, o serviço de lançamento não pode ser impactado caso o consolidador esteja indisponível.


Manter um único banco geraria contenção e impacto de performance.

## Decisão
Utilizar bancos separados:

- Banco transacional relacional (ex.: PostgreSQL/MySQL) será utilizado como system of record para lançamentos.
- Para consolidação, será adotado um banco otimizado para leitura e agregação, como um repositório analítico (ex.: PostgreSQL com particionamento, ClickHouse, Elastic, ou Data Warehouse).
- A sincronização entre eles será feita via eventos.

## Alternativas Consideradas
Banco único relacional
- prós: simplicidade
- contras: lock, impacto entre domínios, baixo desempenho analítico

Data lake desde o início
- prós: escalabilidade futura
- contras: complexidade desnecessária agora

## Consequências
- A consolidação passa a operar sob consistência eventual, o que é aceitável para o contexto, enquanto o domínio de lançamentos mantém consistência forte.
- maior independência entre domínios
- otimização de cada workload
- necessidade de sincronização por eventos

## Riscos e Mitigações
- Risco: divergência entre bases
- Mitigação: consistência eventual com reprocessamento de eventos
- Mitigação adicional: uso de padrão Outbox e reprocessamento a partir de DLQ para reconstrução da base analítica.
