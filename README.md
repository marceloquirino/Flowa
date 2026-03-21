# 📊 Sistema de Gestão de Ordens (FIX 4.4) - Desafio Flowa

> This is a challenge by Coodesh

Este projeto implementa um **Order Management System (OMS) simplificado**, utilizando o protocolo Financial Information eXchange (FIX 4.4), simulando a comunicação entre componentes de um ambiente real de trading.

A solução demonstra o fluxo completo de envio, processamento e resposta de ordens financeiras utilizando mensageria baseada em FIX.

---

## 🧩 Arquitetura da Solução

A aplicação é composta por quatro componentes principais:

### 🔹 OrderGenerator - Web (ASP.NET Core MVC / Razor Pages)
Interface web responsável por:
- Entrada de ordens (símbolo, lado, preço e quantidade)
- Validação de dados
- Envio das requisições para a API

---

### 🔹 OrderGenerator - API (ASP.NET Core Web API)
Responsável por:
- Receber requisições REST (`POST`)
- Validar as ordens
- Construir mensagens FIX (`NewOrderSingle - 35=D`)
- Enviar para processamento
- Retornar a exposição calculada

---

### 🔹 OrderAccumulator (Worker Service)
Serviço responsável por:
- Receber mensagens FIX
- Processar ordens
- Calcular exposição por ativo
- Retornar respostas via FIX (`ExecutionReport`)

---

### 🔹 Integração FIX Engine
A comunicação entre os serviços é feita utilizando o QuickFIX/n, seguindo a especificação FIX 4.4.

---

## 🔄 Fluxo de Execução

1. O usuário envia uma ordem pela interface web
2. A API valida e cria uma mensagem FIX (`NewOrderSingle`)
3. A mensagem é enviada via FIX para o OrderAccumulator
4. O OrderAccumulator processa e atualiza a exposição
5. Um `ExecutionReport` é retornado
6. O usuário recebe a exposição atualizada

---

## 📚 Documentação da API

A API conta com documentação interativa via Swagger, aberta automaticamente ao iniciar o projeto.

---

## 🛠️ Tecnologias Utilizadas

- C#
- .NET 10
- ASP.NET Core MVC (Razor Pages)
- ASP.NET Core Web API
- Worker Service
- QuickFIX/n (FIX 4.4)
- Swashbuckle (Swagger)
- xUnit (Testes unitários)
- NSubstitute (mocking)
- Serilog (logging)
- OpenTelemetry (Observabilidade)
- Scruttor (Decorator)
- Newtonsoft (json)

---

## 🧪 Testes

O projeto inclui testes unitários para:

- OrderGenerator API (`TestOrderGenerator`)
- OrderAccumulator (`TestOrderAccumulator`)

---

## 📊 Observabilidade

- Implementação de observabilidade com OpenTelemetry, incluindo tracing distribuído e métricas de latência e volume de ordens
- Uso do padrão Decorator para desacoplar a telemetria da lógica de negócio, garantindo maior manutenibilidade e aderência ao princípio de responsabilidade única (SRP)

---

## 🚀 Como Executar o Projeto

1. Clone o repositório
2. Abra a solução no Visual Studio 2026
3. Configure múltiplos projetos de inicialização:
   - `OrderAccumulator`
   - `OrderGenerator.Api`
   - `OrderGenerator.Web`
4. Execute a aplicação

---

## 🖥️ Interface

- **Home:** página inicial
- **Ordens:** envio de ordens com validação completa

### Feedback ao usuário:
- ✅ Sucesso: Toastr verde com exposição
- ❌ Erro: Toastr vermelho com mensagem

---

## 📁 Logs e Persistência

Durante a execução, são gerados arquivos (ignorados no `.gitignore`):

### 📌 Diretórios
- `store/` → Persistência da sessão FIX (mensagens e sequência)
- `logs/` → Eventos e mensagens FIX

### 📌 Destaque
- Logs de exposição (ex: `log20260320.txt`)
- Arquivos não devem ser abertos durante execução (uso ativo)

---

## ⚠️ Comportamento

- As exposições são mantidas em memória
- Ao reiniciar o sistema, os dados são resetados

---

## 💡 Melhorias Possíveis

Em um cenário real, seria recomendado:

- 📦 Criar um projeto `SharedModels` para reutilização de contratos
- 🗄️ Persistir dados (exposição e ordens) em banco de dados
- 🔧 Tornar os símbolos configuráveis (em vez de `enum`)
- 🔄 Implementar resiliência e retry entre serviços
- 🔐 Implementar autenticação e autorização, garantindo que as ordens e exposições sejam separadas por usuário

---

## 📌 Considerações Finais

Este projeto demonstra, de forma prática, a utilização do protocolo FIX em uma arquitetura moderna com .NET, abordando conceitos como:

- Comunicação entre sistemas financeiros
- Processamento assíncrono
- Separação de responsabilidades
- Testabilidade
- Observabilidade
