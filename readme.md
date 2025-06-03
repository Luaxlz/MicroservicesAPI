# MicroservicesAPI

Este projeto demonstra a implementação de microserviços utilizando .NET, com foco em comunicação síncrona e assíncrona, deploy em Kubernetes e uso de API Gateway.

## 📚 Sumário

- [Visão Geral](#visão-geral)
- [Arquitetura](#arquitetura)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Como Executar](#como-executar)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Contribuindo](#contribuindo)
- [Licença](#licença)

## 🔍 Visão Geral

O objetivo deste projeto é demonstrar:

- Criação de microserviços com .NET
- Comunicação entre serviços via HTTP/gRPC
- Mensageria assíncrona com RabbitMQ
- Deploy em cluster Kubernetes
- Uso de API Gateway para roteamento

## 🧱 Arquitetura

![Diagrama de Arquitetura](docs/architecture-diagram.png)

A arquitetura segue os princípios de Clean Architecture, com separação clara entre camadas e responsabilidades.

## 🛠️ Tecnologias Utilizadas

- .NET 8
- ASP.NET Core
- gRPC
- RabbitMQ
- Docker & Kubernetes
- Ocelot API Gateway
- Entity Framework Core

## 🚀 Como Executar

1. Clone o repositório:

   ```bash
   git clone https://github.com/Luaxlz/MicroservicesAPI.git
   cd MicroservicesAPI
   ```

2. Inicie os serviços com Docker Compose:

   ```bash
   docker-compose -f docker/docker-compose.yml up --build
   ```

3. Acesse a aplicação em `http://localhost:5000`.

## 📁 Estrutura do Projeto

A estrutura do projeto segue o padrão:

- `src/ServiceName.API`: Contém os controladores e configurações da API.
- `src/ServiceName.Application`: Contém a lógica de aplicação e interfaces.
- `src/ServiceName.Domain`: Contém as entidades e regras de negócio.
- `src/ServiceName.Infrastructure`: Contém a implementação de repositórios e acesso a dados.

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor, leia o [CONTRIBUTING.md](CONTRIBUTING.md) para mais detalhes sobre nosso código de conduta e o processo de envio de pull requests.

## 📄 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.
