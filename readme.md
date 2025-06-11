# 🚀 Microservices API - Projeto de Aprendizado

Um projeto completo para aprender e consolidar os fundamentos de **.NET Web API**, **Microserviços**, **Docker**, **Kubernetes** e tecnologias relacionadas.

## 📋 Sobre o Projeto

Este repositório contém uma arquitetura de microserviços desenvolvida para fins educacionais, cobrindo conceitos essenciais como:

- Desenvolvimento de Web APIs com ASP.NET Core
- Arquitetura de Microserviços
- Containerização com Docker
- Orquestração com Kubernetes
- Comunicação entre serviços
- Persistência de dados
- Monitoramento e logging

## 🏗️ Arquitetura

```
MicroservicesAPI/
├── PlatformService/          # Serviço de gerenciamento de plataformas
├── CommandsService/          # Serviço de comandos
├── k8s/                      # Manifestos Kubernetes
├── docker-compose.yml        # Orquestração local
├── .gitignore
└── README.md
```

## 🛠️ Tecnologias Utilizadas

### Backend

- **.NET 8** - Framework principal
- **ASP.NET Core Web API** - Criação de APIs RESTful
- **Entity Framework Core** - ORM para acesso a dados
- **SQL Server / PostgreSQL** - Banco de dados

### DevOps & Infraestrutura

- **Docker** - Containerização
- **Kubernetes** - Orquestração de containers
- **Docker Compose** - Desenvolvimento local
- **GitHub Actions** - CI/CD (planejado)

### Ferramentas

- **Swagger/OpenAPI** - Documentação da API
- **Postman** - Testes de API
- **Visual Studio Code** - Editor
- **Git** - Controle de versão

## 🚦 Como Executar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [kubectl](https://kubernetes.io/docs/tasks/tools/) (para Kubernetes)

### Executando Localmente

#### 1. Clone o repositório

```bash
git clone https://github.com/[seu-usuario]/MicroservicesAPI.git
cd MicroservicesAPI
```

#### 2. Execute o PlatformService

```bash
cd PlatformService
dotnet restore
dotnet run
```

Acesse: `http://localhost:5000`

#### 3. Execute o CommandsService

```bash
cd CommandsService
dotnet restore
dotnet run
```

Acesse: `http://localhost:6000`

### Executando com Docker

#### Build das imagens

```bash
# PlatformService
cd PlatformService
docker build -t platformservice .

# CommandsService
cd CommandsService
docker build -t commandsservice .
```

#### Executar com Docker Compose

```bash
docker-compose up -d
```

### Deploy no Kubernetes

```bash
# Aplicar manifestos
kubectl apply -f k8s/

# Verificar pods
kubectl get pods

# Verificar serviços
kubectl get services
```

## 📚 Funcionalidades Implementadas

### ✅ PlatformService

- [x] CRUD de plataformas
- [x] API RESTful
- [x] Swagger/OpenAPI
- [ ] Persistência com Entity Framework
- [ ] Docker containerization
- [ ] Kubernetes deployment

### ✅ CommandsService

- [x] Estrutura básica da API
- [x] Controller de teste
- [ ] CRUD de comandos
- [ ] Comunicação com PlatformService
- [ ] Message Bus (RabbitMQ)
- [ ] Docker containerization

### 🔄 Recursos Planejados

- [ ] **Autenticação JWT**
- [ ] **Message Bus** (RabbitMQ/Azure Service Bus)
- [ ] **API Gateway**
- [ ] **Health Checks**
- [ ] **Logging centralizado** (Serilog + ELK Stack)
- [ ] **Monitoring** (Prometheus + Grafana)
- [ ] **CI/CD Pipeline** (GitHub Actions)
- [ ] **Testes unitários e integração**

## 🎯 Endpoints da API

### PlatformService (`http://localhost:5000`)

| Método | Endpoint              | Descrição                  |
| ------ | --------------------- | -------------------------- |
| GET    | `/api/platforms`      | Lista todas as plataformas |
| GET    | `/api/platforms/{id}` | Busca plataforma por ID    |
| POST   | `/api/platforms`      | Cria nova plataforma       |
| PUT    | `/api/platforms/{id}` | Atualiza plataforma        |
| DELETE | `/api/platforms/{id}` | Remove plataforma          |

### CommandsService (`http://localhost:6000`)

| Método | Endpoint                  | Descrição                  |
| ------ | ------------------------- | -------------------------- |
| POST   | `/api/commands/platforms` | Teste de conexão           |
| GET    | `/api/commands`           | Lista comandos (planejado) |
| POST   | `/api/commands`           | Cria comando (planejado)   |

## 📖 Conceitos Aprendidos

### Microserviços

- **Decomposição de domínio** - Separação por contexto de negócio
- **Comunicação síncrona** - HTTP/REST APIs
- **Comunicação assíncrona** - Message queues
- **Data sovereignty** - Cada serviço tem seu próprio banco

### .NET Core

- **Minimal APIs** vs **Controller-based APIs**
- **Dependency Injection**
- **Configuration management**
- **Middleware pipeline**
- **Entity Framework Core**

### Docker & Kubernetes

- **Multi-stage builds**
- **Container orchestration**
- **Service discovery**
- **Load balancing**
- **Rolling deployments**

## 🔧 Estrutura de Desenvolvimento

### Branch Strategy

- `main` - Código estável
- `develop` - Desenvolvimento ativo
- `feature/*` - Novas funcionalidades
- `hotfix/*` - Correções urgentes

### Padrões de Commit

```
feat: adiciona nova funcionalidade
fix: corrige bug
docs: atualiza documentação
refactor: refatora código
test: adiciona testes
```

## 📝 Notas de Aprendizado

### Desafios Encontrados

1. **Configuração de portas** - Conflitos entre serviços
2. **HTTPS Redirection** - Warnings em desenvolvimento
3. **Docker networking** - Comunicação entre containers

### Lições Aprendidas

1. **Swagger é essencial** para documentação e testes
2. **Docker Compose simplifica** o desenvolvimento local
3. **Kubernetes requer** planejamento cuidadoso de recursos

## 🤝 Como Contribuir

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'feat: adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📚 Recursos de Estudo

### Documentação Oficial

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Docker Documentation](https://docs.docker.com/)
- [Kubernetes Documentation](https://kubernetes.io/docs/)

### Cursos Recomendados

- [.NET Microservices Course](https://www.udemy.com/course/dotnet-core-microservices/)
- [Kubernetes for Developers](https://kubernetes.io/training/)

### Livros

- "Microservices Patterns" - Chris Richardson
- "Docker Deep Dive" - Nigel Poulton
- "Kubernetes in Action" - Marko Lukša

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👨‍💻 Autor

**Lucas Angeli**

- GitHub: [@[seu-usuario]](https://github.com/[seu-usuario])
- LinkedIn: [Seu LinkedIn](https://linkedin.com/in/[seu-perfil])

---

⭐ Se este projeto te ajudou a aprender, considere dar uma estrela!

## 📊 Status do Projeto

![Status](https://img.shields.io/badge/Status-Em%20Desenvolvimento-yellow)
![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![Kubernetes](https://img.shields.io/badge/Kubernetes-In%20Progress-orange)
