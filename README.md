# Controle de Fluxo de Caixa -- Arquitetura de Soluções

## Visão Geral

Este projeto demonstra a definição de uma arquitetura de soluções para
controle de fluxo de caixa diário, contemplando lançamentos financeiros
(débitos e créditos) e a geração de um consolidado diário de saldo.

O foco está na tomada de decisões arquiteturais, decomposição de
domínios, definição de capacidades de negócio e atendimento a requisitos
funcionais e não funcionais como escalabilidade, resiliência,
disponibilidade e segurança.

------------------------------------------------------------------------

## Objetivos da Solução

-   Garantir alta disponibilidade do serviço de lançamentos
-   Permitir geração de relatório diário consolidado
-   Garantir que falhas no consolidado não impactem os lançamentos
-   Projetar solução escalável, resiliente e evolutiva

------------------------------------------------------------------------

## Visão Arquitetural

A solução utiliza arquitetura orientada a eventos com microsserviços,
garantindo desacoplamento entre domínios e tolerância a falhas.

### Estilo Arquitetural

-   Microsserviços
-   Event-driven
-   APIs REST
-   Comunicação assíncrona

------------------------------------------------------------------------

## Domínios e Capacidades

### Lançamentos Financeiros (Core Domain)

Capacidades: - Registrar créditos e débitos - Validar regras de
negócio - Persistir dados - Publicar eventos

Responsabilidades: - Fonte da verdade - Forte consistência - Alta
disponibilidade

------------------------------------------------------------------------

### Consolidação Diária (Supporting Domain)

Capacidades: - Consumir eventos - Agrupar por data - Calcular saldo
diário - Expor relatórios

Responsabilidades: - Processamento assíncrono - Consistência eventual -
Tolerância a falhas

------------------------------------------------------------------------

## Comunicação entre Serviços

  Origem        Destino        Tipo     Justificativa
  ------------- -------------- -------- ------------------------------
  Cliente       Lançamentos    REST     Operações críticas
  Lançamentos   Consolidação   Evento   Desacoplamento e resiliência

------------------------------------------------------------------------

## Requisitos Não Funcionais

-   Serviço de lançamentos independente do consolidado
-   Consolidação suporta até 50 req/s
-   Tolerância a até 5% de perda
-   Segurança com OAuth2 e JWT

------------------------------------------------------------------------

## Arquitetura Alvo
![Arquitetura](docs/IMG/Arquitetura.png)

------------------------------------------------------------------------

## Tecnologias

-   Backend: .NET / Java / Node
-   Mensageria: Kafka ou RabbitMQ
-   Banco Lançamentos: PostgreSQL / SQL Server
-   Banco Consolidação: PostgreSQL / MongoDB
-   Infra: Docker / Docker Compose

------------------------------------------------------------------------
## Decisões Arquiteturais (ADRs)

As principais decisões de arquitetura do sistema estão documentadas nos Architecture Decision Records:

- [ADR-001 – Adoção de Arquitetura Orientada a Eventos](docs/ADR/ADR-0001-Arquitetura-Orientada-a-Eventos.md)
- [ADR-002 – Separação entre banco transacional e banco de consolidação](docs/ADR/ADR-002-Separacao-de-bancos.md)
- [ADR-003 – Escolha da plataforma de mensageria](docs/ADR/ADR-003-Mensageria.md)

------------------------------------------------------------------------
## Execução Local

docker-compose up -d

------------------------------------------------------------------------

## Estimativa de Custos de Infraestrutura

A seguir, uma estimativa aproximada de custos mensais considerando implantação em ambiente cloud (AWS ou Azure), com carga baixa a moderada e ambiente produtivo simplificado. Os valores variam por região e contrato, sendo apresentados apenas como referência arquitetural.

### Componentes considerados

- 2 serviços Web API (.NET)
  - Lancamentos.Api
  - Consolidado.Api
- Serviço de mensageria
- Banco de dados operacional para lançamentos
- Banco de leitura para consolidação
- Monitoramento e logs básicos

### Estimativa aproximada (mensal)

| Componente                                          | Opção sugerida  | Custo aproximado |
|-----------------------------------------------------|----------------------------------------|-----------|
| Hospedagem APIs                                     | 2 x containers (B1s / F1)              | USD 20–40 |
| Banco relacional lançamentos                        | Azure SQL Basic / RDS t4g.micro        | USD 25–40 |
| Banco leitura consolidação                          | Azure SQL Basic / RDS t4g.micro        | USD 25–40 |
| Mensageria                                          | Azure Service Bus Basic / SQS Standard | USD 5–15  |
| Armazenamento de logs                               | Application Insights / CloudWatch      | USD 10–25 |
| Monitoramento e métricas                            | incluído parcial / +USD 10             | USD 0–10  |
| Trafego / egress                                    | baixo volume                           | USD 5–15  |
|-----------------------------------------------------|----------------------------------------|-----------|

**Total estimado:** entre **USD 90 e USD 180 por mês**

### Observações de otimização

- ambientes de desenvolvimento podem usar **in-memory** ou SQLite com custo zero
- em produção é recomendado:
  - storage redundante
  - backup automático
  - zona de disponibilidade
- autoscaling só é ativado em picos, impactando custo variável
- uso de containers reduz custo versus VMs

### Possíveis reduções

- Serverless (Azure Functions / AWS Lambda) para consolidação diária
- banco único particionado por domínio (não recomendado, mas possível)
- desligamento automático fora de horário comercial
- logs com retenção menor

### Possível aumento de custo

- alta disponibilidade multi-zona
- mensageria com garantia forte (Premium / FIFO)
- criptografia gerenciada por HSM
- auditoria avançada e SIEM

------------------------------------------------------------------------

## Considerações Finais

Projeto focado em clareza arquitetural e boas práticas, priorizando
decisões conscientes em detrimento de implementação completa.

------------------------------------------------------------------------

## Autor

Jorge Luis Oliveira Barros
