# ADR-003 – Escolha da Plataforma de Mensageria

## Status  
Aceito

## Contexto  
A arquitetura definida depende de comunicação assíncrona entre o serviço de lançamentos e o serviço de consolidação diária.

O sistema precisa suportar:

- picos de 50 requisições por segundo no consolidador
- até 5% de perda aceitável de requisições
- desacoplamento entre domínios
- reprocessamento de eventos
- tolerância a falhas
- DLQ e política de retry

Além disso, o serviço de lançamentos não deve ficar indisponível caso o consolidador falhe, o que exige bufferização e desacoplamento por mensageria.

## Decisão  
Será adotada uma plataforma de mensageria robusta de mercado, com suporte a:

- alta vazão
- particionamento
- retenção de mensagens
- reprocessamento
- DLQ
- consumidores paralelos

Foram considerados dois padrões principais:

- **Kafka** – indicado para streaming de eventos, alto volume e retenção longa com reprocessamento histórico
- **RabbitMQ** – indicado para filas tradicionais com roteamento, troca de mensagens e garantia ponto-a-ponto

A arquitetura será orientada a eventos independentemente da ferramenta específica.  
A definição final entre Kafka ou RabbitMQ dependerá do contexto do cliente, considerando infraestrutura existente, custos e maturidade da equipe.

## Critérios de Escolha  

A seleção da plataforma deverá considerar:

- volume e taxa de eventos
- necessidade de retenção histórica
- complexidade operacional aceitável
- custos de infraestrutura e licenciamento
- requisitos de auditoria e regulatórios
- experiência do time com a tecnologia
- latência e throughput necessários

## Alternativas Consideradas  

### 1) Fila em memória na aplicação  
Descartada por:

- falta de resiliência
- perda de dados em falha de instância
- ausência de DLQ
- escalabilidade limitada

### 2) Comunicação REST síncrona entre serviços  
Descartada por:

- forte acoplamento
- impacto do consolidador sobre lançamentos
- violação direta do requisito de independência
- maior suscetibilidade a cascatas de falha

## Consequências  

Positivas:

- escalabilidade horizontal dos consumidores
- desacoplamento entre serviços
- suporte a picos de demanda
- processamento assíncrono e resiliente

Negativas:

- maior complexidade operacional
- necessidade de monitoramento específico
- gestão de DLQ e retries

Consequência arquitetural importante:

- consumidores devem ser idempotentes
- ordenação pode não ser garantida entre partições
- é necessário definir chaves de particionamento

## Riscos e Mitigações  

Risco: acúmulo de mensagens (backpressure)  
Mitigação: auto-scaling de consumidores e alarmes por lag

Risco: crescimento indefinido de DLQ  
Mitigação: processos de reprocessamento controlado e playbooks operacionais

Risco: indisponibilidade do cluster de mensageria  
Mitigação: implantação distribuída, replicação e múltiplos brokers
