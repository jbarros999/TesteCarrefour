## Arquitetura Alvo

A solução é composta por dois serviços principais:

1. **Serviço de Lançamentos**
   - responsável por registrar débitos e créditos
   - opera sobre banco transacional
   - publica eventos de "lançamento registrado" na mensageria

2. **Serviço de Consolidação Diária**
   - consome eventos de lançamentos
   - processa agregações por dia
   - persiste consolidação em banco orientado à leitura

### Componentes de apoio

- **Plataforma de Mensageria**
  - garante comunicação assíncrona
  - desacopla consolidação de lançamentos
  - permite reprocessamento e DLQ

- **API Gateway**
  - expõe APIs aos clientes
  - aplica segurança, rate limit e observabilidade

- **Observabilidade**
  - logs centralizados
  - métricas
  - tracing distribuído

### Bancos de Dados

- banco transacional (OLTP) — para lançamentos
- banco analítico (OLAP ou read-optimized) — para consolidação diária

### Motivadores Arquiteturais

- escalabilidade independente por domínio
- resiliência: consolidador pode falhar sem parar lançamentos
- processamento assíncrono
- separação clara de responsabilidades
