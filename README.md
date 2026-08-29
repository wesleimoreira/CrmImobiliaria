# 🏢 CRM Imobiliária

Um sistema de gestão completo para imobiliárias, desenvolvido em **.NET 10 com Blazor**, cobrindo o ciclo completo da operação: comercial, locação, loteamentos e comissões.

## ✨ Principais Características

### 📊 Módulos de Gestão
- **Comercial**: Clientes, corretores, imóveis, anúncios, funil de vendas, propostas e fechamento de vendas
- **Locação**: Contratos, pagamentos de aluguel, repasse ao proprietário, vistorias e manutenções
- **Loteamentos**: Empreendimentos, lotes, controle de status, espelho visual, simulador de parcelamento
- **Comissões**: Cálculo automático por regras e rateio entre corretores
- **Financeiro**: Repasses, despesas, controle de fluxo de caixa

### 🎯 Diferenciais
- ✅ Licença de uso **vitalícia** (pagamento único, sem mensalidade)
- ✅ Hospedagem sob **seu domínio** (não dependência de provedores)
- ✅ Todos os 17 módulos inclusos desde o início
- ✅ Suporte direto (sem central de atendimento terceirizada)
- ✅ Sem multa de fidelidade ou cancelamento

## 🛠️ Stack Tecnológico

- **Backend**: .NET 10, Entity Framework Core, MSSQL Server
- **Frontend**: Blazor Interactive Server
- **UI Components**: MudBlazor
- **Arquitetura**: Clean Architecture (Domain-Driven Design)
- **Pattern**: CQRS com MediatR
- **Containerização**: Docker & Docker Compose

## 📦 Arquitetura

```
CrmImobiliaria.Domain/          # Modelos de domínio, Value Objects e regras de negócio
CrmImobiliaria.Application/     # Use Cases, Commands, Queries, Handlers
CrmImobiliaria.Infrastructure/  # Persistência, Migrations, Contexto do BD
CrmImobiliaria.Web/             # Componentes Blazor, UI, Endpoints
landing/                         # Landing page estática (HTML puro)
proxy/                           # Configuração Caddy (reverse proxy + HTTPS)
```

## 🚀 Quick Start

### Pré-requisitos
- Docker & Docker Compose
- .NET 10 SDK (para desenvolvimento local)
- SQL Server (ou use via Docker)

### Desenvolvimento Local

```bash
# Clone o repositório
git clone https://github.com/wesleimoreira/CrmImobiliaria.git
cd CrmImobiliaria

# Configure variáveis de ambiente
cp .env.example .env

# Inicie os serviços (banco + aplicação)
docker-compose up -d

# Acesse em: http://localhost:5267
```

### Deploy em Produção

```bash
# Configure seu .env com dados reais
# Atualize proxy/Caddyfile com seus domínios
# Configure DNS apontando para seu servidor

# Inicie em produção
docker-compose up -d
```

Veja [SECURITY_CHECKLIST.md](./SECURITY_CHECKLIST.md) para instruções detalhadas de segurança.

## 📋 Fluxos Principais

### Venda de Imóvel
1. Cadastro de cliente e imóvel
2. Criação de proposta
3. Aceitação e fechamento
4. Cálculo automático de comissão

### Locação
1. Cadastro do contrato
2. Controle de pagamentos
3. Repasse automático ao proprietário
4. Vistoria de entrada e saída

### Loteamento
1. Cadastro de empreendimento e lotes
2. Reserva de lote com prazo
3. Conversão de reserva em venda
4. Simulador de parcelamento

## 🔐 Segurança

O repositório foi auditado e removido de dados sensíveis:
- ✅ Sem credenciais ou senhas reais
- ✅ Sem domínios/emails pessoais
- ✅ Sem informações de infraestrutura específica
- ✅ `.gitignore` bem configurado

**Antes de fazer deploy**, consulte [SECURITY_CHECKLIST.md](./SECURITY_CHECKLIST.md) para configurar variáveis de ambiente e segurança.

## 📚 Estrutura de Pastas

```
.
├── CrmImobiliaria.Domain/           # Lógica de domínio
│   ├── Entities/                    # Cliente, Imóvel, Contrato, etc
│   ├── ValueObjects/                # Email, Telefone, CPF, etc
│   └── Events/                      # Eventos de domínio
├── CrmImobiliaria.Application/      # Casos de uso
│   ├── Clientes/
│   ├── Imoveis/
│   ├── Vendas/
│   ├── Locacoes/
│   └── Comissoes/
├── CrmImobiliaria.Infrastructure/   # Dados e infraestrutura
│   ├── Persistence/
│   └── Migrations/
├── CrmImobiliaria.Web/              # Apresentação (Blazor)
│   ├── Components/
│   ├── Pages/
│   └── Services/
├── landing/                         # Landing page
├── proxy/                           # Reverse proxy (Caddy)
├── docker-compose.yml               # Orquestração
└── .env.example                     # Template de configuração
```

## 🗄️ Banco de Dados

- **Engine**: SQL Server 2022
- **Migrations**: Entity Framework Core
- **Seeders**: Papéis (roles) e usuário admin padrão

As migrações são aplicadas automaticamente na primeira execução.

## 🔑 Principais Entidades

- **Cliente**: Pessoa física/jurídica interessada em compra, locação ou loteamento
- **Imóvel**: Propriedade cadastrada para venda ou locação
- **Contrato**: Venda ou locação de imóvel
- **Lote**: Unidade de terreno em loteamento
- **Comissão**: Cálculo automático baseado em regras configuráveis
- **Repasse**: Pagamento ao proprietário pela locação

## 📝 Configuração

### Variáveis de Ambiente

```env
# Banco de dados
MSSQL_SA_PASSWORD=SenhaForte#2026

# Aplicação
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__CrmImobiliaria=Server=db;Database=CrmImobiliaria;...

# Seed de dados
SEED_ADMIN_EMAIL=admin@seu-dominio.com.br
SEED_ADMIN_PASSWORD=SenhaForte#2026
```

## 🤝 Contribuindo

Este é um projeto de portfólio. Se deseja colaborar ou sugerir melhorias, sinta-se livre para abrir issues ou pull requests!

## 📄 Licença

Este projeto é fornecido como está, para fins de portfólio e demonstração.

## 👤 Sobre

Desenvolvido como um sistema real para imobiliárias, focando em simplicidade de uso e funcionalidades essenciais para operações de tamanho pequeno e médio.

---

**📞 Dúvidas sobre deployment?** Veja [SECURITY_CHECKLIST.md](./SECURITY_CHECKLIST.md)
