# Sistema de Gestão de Ordens (FIX 4.4) - Desafio Flowa

>  This is a challenge by [Coodesh](https://coodesh.com/)

Este projeto implementa um sistema simplificado de Order Management System (OMS) utilizando o protocolo Financial Information eXchange (FIX) Protocol, simulando a comunicação entre componentes de um ambiente real de trading.

A solução é composta por quatro partes principais:
- OrderGenerator - Web (ASP.NET Core MVC - Razor Pages): Interface web que permite ao usuário criar e enviar ordens (símbolo, lado, preço e quantidade).
- OrderGenerator - Api (ASP.NET Core Web API): Serviço responsável por receber a solicitação as solicitações de criação de ordens via post (REST), validar, enviar mensagens FIX para o processamento das ordens e retornar a exposição calculada. 
- OrderAccumulator (Work Service): Serviço responsável por receber mensagens FIX, processar ordens, calcular a exposição e retorná-la.
- Integração com FIX Engine: A comunicação entre os componentes é feita utilizando o QuickFIX/n, seguindo a especificação FIX 4.4.

A solução apresenta testes unitários para o projeto OrderGenerator API

## Como funciona
- O usuário envia uma ordem pela interface web.
- O OrderGenerator valida a mensagem e monta uma mensagem FIX NewOrderSingle (35=D).
- A mensagem é enviada via FIX para o OrderAccumulator.
- O OrderAccumulator processa a ordem e atualiza a exposição.
- Uma resposta (ExecutionReport) é enviada para confirmar o processamento.
- O usuário recebe a exposição atual.
- Também é utilizado Swagger para fornecer ao desenvolvedo uma documentação de API do OrderGenerator - A página é aberta automaticamente ao rodar o sistema

## Tecnologias
- C# 14
- .NET 10 / ASP.NET Core MVC
- .NET 10 / ASP.NET Core Web API
- .NET 10 / Work Service 
- .NET 10 / xUnit Test
- Razor Pages
- QuickFix.Net.NETCore.FIX44 1.8.1
- Swashbuckle.AspNetCore 10.1.5
- NSubstitute 6.0.0-rc.1
- Serilog.AspNetCore 10.0.0

## Como instalar e usar o projeto
- Clonar o projeto
- Abrir o projeto Flowa.slnx pelo Visual Studio 2026
- No Visual Studio, configurar projetos de inicialização
- Selecionar "Vários projetos de inicialização". Na coluna Ação, colocar para Iniciar: OrderAccumulator, OrderGenerator.Api e OrderGenerator.Web
- Iniciar o projeto. Isso abrirá terminais e 2 páginas Web (Front-end e Swagger)
- Página do Front-end: possui duas telas: Tela, inicial, de boas vindas (Home) e tela para execução de ordens (Ordens). Na tela de ordens selecione e preencha todos os campos (todos possuem validações para que o usuário não envie dados incorretos) e clique em Enviar Ordem. Caso tenha sucesso irá aparecer um Toastr verde indicando sucesso e apresentando a exposição para o símbolo enviado, esse Toastr irá desaparecer após alguns segundos. Caso ocorra alguma falha, irá aparecer um Toastr vermelho indicando o erro.
- Alguns logs serão exibidos no terminais.
- Alguns logs serão escritos em arquivos (todos são ignorados no .gitignore): OrderAccumulator\store e OrderGenerator\store (armazenam mensagens e sequência da sessão FIX para recuperação e controle). OrderAccumulator\logs e OrderGenerator\logs (logs das mensagens e eventos da sessão FIX). Além disso, na pasta OrderAccumulator\logs terá um log (que o nome é configurado por dia, ex: log20260320.txt) listando, principalmente as exposições. Essas arquivos estão em uso durante o processamento do sistema, então é recomendado que não sejam abertos durante a execução.
- Se o sistema for reiniciado, as exposições também serão reiniciadas
