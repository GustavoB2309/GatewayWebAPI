# 💳 GatewayWebAPI - Módulo de Pagamento e Checkout Transparente

Este repositório contém uma **Web API de Gateway de Pagamento de Alta Performance** desenvolvida em **.NET 10.0**, projetada sob os conceitos de arquitetura limpa, segurança lógica de dados e persistência resiliente.

## 🚀 Tecnologias Utilizadas
* **C# & ASP.NET Core Minimal APIs:** Para construção de endpoints leves, rápidos e de baixa latência.
* **SQL Server Express:** Motor de banco de dados relacional para armazenamento seguro e permanente.
* **Entity Framework Core (EF Core):** Mapeador Objeto-Relacional (ORM) utilizado com Injeção de Dependência.
* **Swagger UI / OpenAPI:** Documentação e interface interativa para testes de integração em ambiente de desenvolvimento.

## 🛡️ Engenharia de Segurança & Regras de Negócio (Compliance)
1. **Cláusulas de Barreira (Early Return):** Validação rígida de entradas nulas, vazias ou strings inválidas (`string.IsNullOrWhiteSpace`).
2. **Blindagem Numérica contra Fraudes:** Proteção algorítmica contra o ataque de "dinheiro infinito" bloqueando requisições com valores menores ou iguais a zero (`<= 0`) ou que estourem o saldo disponível.
3. **Tratamento Global de Falhas Físicas:** Implementação de blocos `try/catch` de infraestrutura, garantindo que panes no servidor de banco de dados não derrubem a aplicação (Resiliência de Sistema).
4. **Segurança de Credenciais:** Desacoplamento completo da String de Conexão do código fonte, armazenando chaves e endereços de rede de forma segura no arquivo `appsettings.json`.

## 📦 Funcionalidades Implementadas
* `POST /cadastrar`: Injeção de novos clientes diretamente no banco de dados físico (SQL Server).
* `POST /pagar`: Fluxo clássico de validação de saldo, processamento matemático e persistência (`SaveChanges`).
* `POST /checkout`: Rota unificada inteligente de "Compra com Um Clique" (Checkout Transparente). Cria o usuário dinamicamente com saldo bônus caso ele não exista no HD e processa a cobrança em um único ciclo de rede.
* **Módulo de Auditoria Temporal (Logs):** Monitoramento assíncrono via console com carimbo de data/hora (`DateTime.Now`) para rastreabilidade de eventos, identificação de alertas (`WARN`) e logs de falhas (`ERRO`).

 Obs: Primeiro projeto intensivo (1 semana e meia) para aprendizado, usei IA como parceira de pareamento de código (Pair Programming), atuando como copiloto técnico. A metodologia consistiu em desafios para lógica algorítmica, aprender conceitos de infraestrutura e (ORM, Injeção de Dependência, Persistência Relacional) e realizar debugging para consolidação de aprendizado.
